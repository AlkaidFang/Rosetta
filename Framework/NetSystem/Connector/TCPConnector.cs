using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Alkaid
{
    public class TCPConnector : INetConnector
    {
        private TcpClient mSocket;

        private IPacketFormat mPacketFormat;

        // 缓冲区
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mWriteBuffer;
        // 临时解析
        private int tempReadPacketLength = 0;
        private int tempReadPacketType = 0;
        private System.IO.MemoryStream tempReadPacketData = null;


        public TCPConnector(IPacketFormat packetFormat)
        {
            mPacketFormat = packetFormat;

            mReadBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主读数据区
            mReadBufferTemp = new Byte[INetConnector.MAX_SOCKET_BUFFER_SIZE]; // 读缓存区
            mWriteBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主写数据区

        }

        public override bool Init()
        {
            return true;
        }

        public override void Tick(float interval)
        {
            doDecodeMessage();

            doSendMessage();
        }

        public override void Destroy()
        {

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
                mIsConnected = false;
                CallbackConnected(mIsConnected);
                return mIsConnected;
            }

            mIsConnected = true;
            mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, new AsyncCallback(RecieveComplete), this);

            CallbackConnected(mIsConnected);

            return mIsConnected;
        }

        public override void SendPacket(IPacket packet)
        {
            Byte[] buffer = null;
            mPacketFormat.GenerateBuffer(ref buffer, packet);

            mWriteBuffer.Push(buffer, buffer.Length);
        }

        private void Disconnect()
        {
            mSocket.GetStream().Close();
            mSocket.Close();
            mIsConnected = false;
            mSocket = null;

            CallbackDisconnected();
        }

        private void doDecodeMessage()
        {
            while (mReadBuffer.DataSize() > 0 && mPacketFormat.CheckHavePacket(mReadBuffer.Buffer(), mReadBuffer.DataSize()))
            {
                // 开始读取
                mPacketFormat.DecodePacket(mReadBuffer.Buffer(), ref tempReadPacketLength, ref tempReadPacketType, ref tempReadPacketData);

                // 逻辑
                NetSystem.Instance.DispatchHandler(tempReadPacketType, tempReadPacketData);

                CallbackRecieved(tempReadPacketType, tempReadPacketData);

                // 偏移
                mReadBuffer.Pop(tempReadPacketLength);
            }
        }

        private void doSendMessage()
        {
            if (mWriteBuffer.DataSize() > 0 && mSocket.GetStream().CanWrite)
            {
                try
                {
                    mSocket.GetStream().BeginWrite(mWriteBuffer.Buffer(), 0, mWriteBuffer.DataSize(), new AsyncCallback(WriteComplete), mWriteBuffer.DataSize());
                }
                catch (Exception e)
                {
                    LoggerSystem.Instance.Error("发送数据错误：" + e.Message);
                    Disconnect();
                }
            }
        }

        private void RecieveComplete(IAsyncResult ar)
        {
            try
            {
                int readLength = mSocket.GetStream().EndRead(ar);
                LoggerSystem.Instance.Info("读取到数据字节数:" + readLength);
                if (readLength > 0)
                {
                    mReadBuffer.Push(mReadBufferTemp, readLength);

                    mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, new AsyncCallback(RecieveComplete), this);
                }
                else
                {
                    // error
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("发生读取错误:" + e.Message);
                Disconnect();
            }

        }

        private void WriteComplete(IAsyncResult ar)
        {
            try
            {
                mSocket.GetStream().EndWrite(ar);
                int sendLength = (int)ar.AsyncState;
                LoggerSystem.Instance.Info("发送数据字节数：" + sendLength);
                if (sendLength > 0)
                {
                    mWriteBuffer.Pop(sendLength);
                }
                else
                {
                    // error
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("发生写入错误：" + e.Message);
                Disconnect();
            }
        }
    }
}
