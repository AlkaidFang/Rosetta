using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Alkaid
{
    public class UDPConnector : INetConnector
    {
        private UdpClient mSocket;
        private IPEndPoint mRemoteEndPoint;


        // 缓冲区
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mSendBuffer;

        // 临时解析
        private int tempReadPacketLength = 0;
        private int tempReadPacketType = 0;
        private System.IO.MemoryStream tempReadPacketData = null;

        private AsyncCallback mReadCompleteCallback;
        private AsyncCallback mSendCompleteCallback;

        public UDPConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mRemoteEndPoint = null;
            mReadBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主读数据区
            mReadBufferTemp = null;
            mSendBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主写数据区
            tempReadPacketLength = 0;
            tempReadPacketType = 0;
            tempReadPacketData = null;

            mReadCompleteCallback = new AsyncCallback(ReadComplete);
            mSendCompleteCallback = new AsyncCallback(SendComplete);
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
            DisConnect();
        }

        public override ConnectionType GetConnectionType()
        {
            return ConnectionType.UDP;
        }

        public override bool Connect(string address, int port)
        {
            base.Connect(address, port);
            mSocket = new UdpClient();
            try
            {
                mRemoteEndPoint = new IPEndPoint(IPAddress.Parse(mNetHoster.GetAddress()), mNetHoster.GetPort());
                mSocket.Connect(mRemoteEndPoint);
                mSocket.DontFragment = true; // 不分段
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error(e.Message);
                mIsConnected = false;
                CallbackConnected(mIsConnected);
                return mIsConnected;
            }
            
            mIsConnected = true;
            CallbackConnected(mIsConnected);
            mSocket.BeginReceive(new AsyncCallback(ReadComplete), this);

            return mIsConnected;
        }

        public override void SendPacket(IPacket packet)
        {
            Byte[] buffer = null;
            mPacketFormat.GenerateBuffer(ref buffer, packet);
            mSendBuffer.Push(buffer, buffer.Length);
        }

        public override void DisConnect()
        {
            if (mIsConnected)
            {
                mSocket.Close();
                mIsConnected = false;
                mSocket = null;

                CallbackDisconnected();
            }
        }

        private void ReadComplete(IAsyncResult ar)
        {
            try
            {
                mReadBufferTemp = mSocket.EndReceive(ar, ref mRemoteEndPoint);
                if (mReadBufferTemp != null && mReadBufferTemp.Length > 0)
                {
                    mReadBuffer.Push(mReadBufferTemp);

                    mSocket.BeginReceive(mReadCompleteCallback, this);
                }
                else
                {
                    //error
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
                int sendLength = mSocket.EndSend(ar);
                if (sendLength > 0)
                {
                    mSendBuffer.Pop(sendLength);
                }
                else
                {
                    //error
                    LoggerSystem.Instance.Error("写入数据为0，将要断开此链接接:" + mNetHoster.ToString());
                    DisConnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("链接：" + mNetHoster.ToString() + ", 发生写入错误：" + e.Message);
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
            if (mIsConnected && mSendBuffer.DataSize() > 0)
            {
                try
                {
                    mSocket.BeginSend(mSendBuffer.Buffer(), mSendBuffer.DataSize(), mSendCompleteCallback, this);
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
