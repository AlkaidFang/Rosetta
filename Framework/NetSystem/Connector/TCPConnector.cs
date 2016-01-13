using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Alkaid
{
    public class TCPConnector : INetConnector
    {
        private TcpClient mSocket;

        // 缓冲区
        private StreamBuffer mNetBuffer;
        // 临时解析
        private int tempReadPacketLength;
        private int tempReadPacketType;
        private System.IO.MemoryStream tempReadPacketData;

        private AsyncCallback mReadCompleteCallback;
        private AsyncCallback mSendCompleteCallback;

        public TCPConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mNetBuffer = new StreamBuffer(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2);
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
            mSocket.GetStream().BeginRead(mNetBuffer.InPipe, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, mReadCompleteCallback, this);

            CallbackConnected(IsConnected());

            return IsConnected();
        }

        public override void SendPacket(IPacket packet)
        {
            Byte[] buffer = null;
            mPacketFormat.GenerateBuffer(ref buffer, packet);

            mNetBuffer.PushStreamOut(buffer);
        }

        public override void DisConnect()
        {
            if (IsConnected())
            {
                mSocket.GetStream().Close();
                mSocket.Close();
                mSocket = null;
                mNetBuffer.Clear();
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
                    mNetBuffer.FinishedIn(readLength);

                    mSocket.GetStream().BeginRead(mNetBuffer.InPipe, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, mReadCompleteCallback, this);
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
                    mNetBuffer.FinishedOut(sendLength);
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
            while (mNetBuffer.StreamInLength > 0 && mPacketFormat.CheckHavePacket(mNetBuffer.StreamIn, mNetBuffer.StreamInLength))
            {
                // 开始读取
                mPacketFormat.DecodePacket(mNetBuffer.StreamIn, ref tempReadPacketLength, ref tempReadPacketType, ref tempReadPacketData);

                mPacketHandlerManager.DispatchHandler(tempReadPacketType, tempReadPacketData);

                CallbackRecieved(tempReadPacketType, tempReadPacketData);

                // 偏移
                mNetBuffer.PopStreamIn(tempReadPacketLength);
            }
        }

        private void doSendMessage()
        {
            int length = mNetBuffer.StreamOutLength;
            if (IsConnected() && length > 0 && mSocket.GetStream().CanWrite)
            {
                try
                {
                    mSocket.GetStream().BeginWrite(mNetBuffer.OutPipe, 0, length, mSendCompleteCallback, length);
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
