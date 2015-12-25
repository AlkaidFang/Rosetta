using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Alkaid
{
    public class TCPConnector : INetConnector
    {
        private TcpClient mSocket;

        // 缓冲区
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mSendBuffer;
        // 临时解析
        private int tempReadPacketLength;
        private int tempReadPacketType;
        private System.IO.MemoryStream tempReadPacketData;

        private AsyncCallback mReadCompleteCallback;
        private AsyncCallback mSendCompleteCallback;


        public TCPConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mReadBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主读数据区
            mReadBufferTemp = new Byte[INetConnector.MAX_SOCKET_BUFFER_SIZE]; // 读缓存区
            mSendBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主写数据区
            tempReadPacketLength = 0;
            tempReadPacketType = 0;
            tempReadPacketData = null;

            mReadCompleteCallback = new AsyncCallback(ReadComplete);
            mSendCompleteCallback = new AsyncCallback(SendComplete);
        }

        public override bool Init()
        {
            base.Init();

            return true;
        }

        public override void Tick(float interval)
        {
            base.Tick(interval);

            doDecodeMessage();

            doSendMessage();
        }

        public override void Destroy()
        {
            base.Destroy();

            DisConnect();
        }

        public override ConnectionType GetConnectionType()
        {
            return ConnectionType.TCP;
        }

        public override bool Connect(string address, int port)
        {
            base.Connect(address, port);
            mSocket = new TcpClient();
            try
            {
                mSocket.Connect(mNetHoster.GetAddress(), mNetHoster.GetPort());
            }
            catch(Exception e)
            {
                LoggerSystem.Instance.Error(e.Message);
                SetConnected(false);
                CallbackConnected(IsConnected());
                return IsConnected();
            }

            SetConnected(true);
            mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, mReadCompleteCallback, this);

            CallbackConnected(IsConnected());

            return IsConnected();
        }

        public override void SendPacket(IPacket packet)
        {
            Byte[] buffer = null;
            mPacketFormat.GenerateBuffer(ref buffer, packet);

            mSendBuffer.Push(buffer);
        }

        public override void DisConnect()
        {
            if (IsConnected())
            {
                mSocket.GetStream().Close();
                mSocket.Close();
                mSocket = null;
                mReadBuffer.Clear();
                mSendBuffer.Clear();
                SetConnected(false);

                CallbackDisconnected();
            }

        }

        private void ReadComplete(IAsyncResult ar)
        {
            try
            {
                int readLength = mSocket.GetStream().EndRead(ar);
                LoggerSystem.Instance.Info("读取到数据字节数:" + readLength);
                if (readLength > 0)
                {
                    mReadBuffer.Push(mReadBufferTemp, readLength);

                    mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, mReadCompleteCallback, this);
                }
                else
                {
                    // error
                    LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:" + mNetHoster.ToString());
                    DisConnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("链接：" + mNetHoster.ToString() + ", 发生读取错误：" + e.Message);
                DisConnect();
            }

        }

        private void SendComplete(IAsyncResult ar)
        {
            try
            {
                mSocket.GetStream().EndWrite(ar);
                int sendLength = (int)ar.AsyncState;
                LoggerSystem.Instance.Info("发送数据字节数：" + sendLength);
                if (sendLength > 0)
                {
                    mSendBuffer.Pop(sendLength);
                }
                else
                {
                    // error
                    DisConnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("发生写入错误：" + e.Message);
                DisConnect();
            }
        }

        private void doDecodeMessage()
        {
            while (mReadBuffer.DataSize() > 0 && mPacketFormat.CheckHavePacket(mReadBuffer.Buffer(), mReadBuffer.DataSize()))
            {
                // 开始读取
                mPacketFormat.DecodePacket(mReadBuffer.Buffer(), ref tempReadPacketLength, ref tempReadPacketType, ref tempReadPacketData);

                mPacketHandlerManager.DispatchHandler(tempReadPacketType, tempReadPacketData);

                CallbackRecieved(tempReadPacketType, tempReadPacketData);

                // 偏移
                mReadBuffer.Pop(tempReadPacketLength);
            }
        }

        private void doSendMessage()
        {
            if (IsConnected() && mSendBuffer.DataSize() > 0 && mSocket.GetStream().CanWrite)
            {
                try
                {
                    mSocket.GetStream().BeginWrite(mSendBuffer.Buffer(), 0, mSendBuffer.DataSize(), mSendCompleteCallback, mSendBuffer.DataSize());
                }
                catch (Exception e)
                {
                    LoggerSystem.Instance.Error("发送数据错误：" + e.Message);
                    DisConnect();
                }
            }
        }
    }
}
