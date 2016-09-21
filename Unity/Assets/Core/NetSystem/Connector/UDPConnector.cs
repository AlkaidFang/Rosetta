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
        private NetStream mNetStream;

        // 临时解析
        private int tempReadPacketLength = 0;
        private int tempReadPacketType = 0;
        private Byte[] tempReadPacketData = null;

        private AsyncCallback mReadCompleteCallback;
        private AsyncCallback mSendCompleteCallback;

        private AsyncThread mSendThread = null;

        public UDPConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mRemoteEndPoint = null;
            mNetStream = new NetStream(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2);
            tempReadPacketLength = 0;
            tempReadPacketType = 0;
            tempReadPacketData = null;

            mReadCompleteCallback = new AsyncCallback(ReadComplete);
            mSendCompleteCallback = new AsyncCallback(SendComplete);

            mSendThread = new AsyncThread(SendLogic);
            mSendThread.Start();
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

            //doSendMessage(); // will do in other thread
        }

        public override void Destroy()
        {
            base.Destroy();

            mSendThread.Stop();

            DisConnect();
        }

        public override ConnectionType GetConnectionType()
        {
            return ConnectionType.UDP;
        }

        public override void Connect(string address, int port)
        {
			SetConnectStatus (ConnectionStatus.CONNECTING);
            base.Connect(address, port);

            mSocket = new UdpClient();
            try
            {
                mRemoteEndPoint = new IPEndPoint(IPAddress.Parse(mRemoteHost.GetAddress()), mRemoteHost.GetPort());
                mSocket.Connect(mRemoteEndPoint);
                mSocket.DontFragment = true; // 不分段
                mSocket.BeginReceive(mReadCompleteCallback, this);
			    SetConnectStatus (ConnectionStatus.CONNECTED);
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error(e.Message);
				SetConnectStatus (ConnectionStatus.ERROR);
            }
            CallbackConnected(IsConnected());
        }

        public override void SendPacket(IPacket packet)
        {
            Byte[] buffer = null;
            mPacketFormat.GenerateBuffer(ref buffer, packet);
            mNetStream.PushOutStream(buffer);
        }

        public override void DisConnect()
        {
            if (IsConnected())
            {
				SetConnectStatus (ConnectionStatus.DISCONNECTED);
                mSocket.Close();
                mSocket = null;
                mNetStream.Clear();

                CallbackDisconnected();
            }
        }

        private void ReadComplete(IAsyncResult ar)
        {
            try
            {
                byte[] mReadTemp = mSocket.EndReceive(ar, ref mRemoteEndPoint);
                if (mReadTemp != null && mReadTemp.Length > 0)
                {
                    mNetStream.PushInStream(mReadTemp);

                    mSocket.BeginReceive(mReadCompleteCallback, this);
                }
                else
                {
                    //error
                    LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:" + mRemoteHost.ToString());
                    DisConnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("链接：" + mRemoteHost.ToString() + ", 发生读取错误：" + e.Message);
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
                    mNetStream.FinishedOut(sendLength);
                }
                else
                {
                    //error
                    LoggerSystem.Instance.Error("写入数据为0，将要断开此链接接:" + mRemoteHost.ToString());
                    DisConnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Error("链接：" + mRemoteHost.ToString() + ", 发生写入错误：" + e.Message);
                DisConnect();
            }
        }

        private void doDecodeMessage()
        {
            while (mNetStream.InStreamLength > 0 && mPacketFormat.CheckHavePacket(mNetStream.InStream, mNetStream.InStreamLength))
            {
                // 开始读取
                mPacketFormat.DecodePacket(mNetStream.InStream, ref tempReadPacketLength, ref tempReadPacketType, ref tempReadPacketData);

                mPacketHandlerManager.DispatchHandler(tempReadPacketType, tempReadPacketData);

                CallbackRecieved(tempReadPacketType, tempReadPacketData);

                // 偏移
                mNetStream.PopInStream(tempReadPacketLength);
            }
        }

        private void SendLogic(AsyncThread thread)
        {
            while (thread.IsWorking())
            {
                doSendMessage();

                System.Threading.Thread.Sleep(30);
            }
        }

        private void doSendMessage()
        {
            int length = mNetStream.OutStreamLength;
            if (IsConnected() && mNetStream.AsyncPipeOutIdle && length > 0)
            {
                try
                {
                    mSocket.BeginSend(mNetStream.AsyncPipeOut, length, mSendCompleteCallback, this);
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
