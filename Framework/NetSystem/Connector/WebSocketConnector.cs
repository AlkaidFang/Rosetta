/**
 * WebSocket4Net未提供异步方法，所以此类需要用Thread封装一下
 * 
 */
using System;
using System.Collections.Generic;
using WebSocket4Net;

namespace Alkaid
{
    public class WebSocketConnector : INetConnector
    {
        WebSocket mSocket;

        // 缓冲区
        private NetStream mNetStream;
        // 临时解析
        private int tempReadPacketLength;
        private int tempReadPacketType;
        private Byte[] tempReadPacketData;

        private AsyncThread mSendThread = null;

        public WebSocketConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mNetStream = new NetStream(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2);
            tempReadPacketLength = 0;
            tempReadPacketType = 0;
            tempReadPacketData = null;

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
            return ConnectionType.WEBSOCKET;
        }

        public override bool Connect(string address, int port)
        {
            base.Connect(address, port);

            string url = mRemoteHost.GetAddress();
            if (port > 0 && !mRemoteHost.GetAddress().Contains(":"))
            {
                url = string.Format("ws://{0}:{1}/", mRemoteHost.GetAddress(), mRemoteHost.GetPort());
            }

            mSocket = new WebSocket(url);
            if (mSocket == null) return false;
            mSocket.EnableAutoSendPing = true;
            mSocket.AutoSendPingInterval = 10;
            mSocket.Opened += new System.EventHandler(OnOpenedCallback);
            mSocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(OnErrorCallback);
            mSocket.Closed += new System.EventHandler(OnClosedCallback);
            mSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(OnMessageReceivedCallback);
            mSocket.DataReceived += new EventHandler<DataReceivedEventArgs>(OnDataReceivedCallback);
            mSocket.Open();

            return true;
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
                SetConnected(false);

                mSocket.Close();
                mSocket = null;
                mNetStream.Clear();

                CallbackDisconnected();
            }
        }

        private void OnOpenedCallback(object sender, EventArgs e)
        {
            SetConnected(true);

            CallbackConnected(IsConnected());
        }

        private void OnErrorCallback(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Exception exc = e.Exception;
            LoggerSystem.Instance.Error(exc.Message);

            DisConnect();

            CallbackError();
        }

        private void OnClosedCallback(object sender, EventArgs e)
        {
            DisConnect();
        }

        private void OnMessageReceivedCallback(object sender, MessageReceivedEventArgs me)
        {
            if (!string.IsNullOrEmpty(me.Message))
            {
                mNetStream.PushInStream(Convert.FromBase64String(me.Message));
            }
            else
            {
                LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:ws://" + mRemoteHost.ToString());
                DisConnect();
            }
        }

        private void OnDataReceivedCallback(object sender, DataReceivedEventArgs de)
        {
            if (de.Data.Length > 0)
            {
                mNetStream.PushInStream(de.Data);
            }
            else
            {
                LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:ws://" + mRemoteHost.ToString());
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
            if (IsConnected() && mNetStream.AsyncPipeOutIdle && length > 0 && mSocket.State == WebSocketState.Open)
            {
                mSocket.Send(mNetStream.AsyncPipeOut, 0, length);
                mNetStream.FinishedOut(length);
            }
        }

    }
}
