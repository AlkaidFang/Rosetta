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
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mSendBuffer;
        // 临时解析
        private int tempReadPacketLength;
        private int tempReadPacketType;
        private System.IO.MemoryStream tempReadPacketData;

        private AsyncThread mSendThread = null;

        public WebSocketConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager) : base(packetFormat, packetHandlerManager)
        {
            mSocket = null;
            mReadBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主读数据区
            mReadBufferTemp = null; // 读缓存区
            mSendBuffer = new TBuffer<Byte>(INetConnector.MAX_SOCKET_BUFFER_SIZE * 2); // 主写数据区
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


            DisConnect();
        }

        public override ConnectionType GetConnectionType()
        {
            return ConnectionType.WEBSOCKET;
        }

        public override bool Connect(string address, int port)
        {
            base.Connect(address, port);

            string url = mNetHoster.GetAddress();
            if (port > 0 && !mNetHoster.GetAddress().Contains(":"))
            {
                url = string.Format("ws://{0}:{1}/", mNetHoster.GetAddress(), mNetHoster.GetPort());
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
            mSendBuffer.Push(buffer);
        }

        public override void DisConnect()
        {
            if (IsConnected())
            {
                mSocket.Close();
                mSocket = null;
                mReadBuffer.Clear();
                mSendBuffer.Clear();
                SetConnected(false);

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
            string msg = me.Message;
            mReadBufferTemp = Convert.FromBase64String(msg);
            if (mReadBufferTemp.Length > 0)
            {
                mReadBuffer.Push(mReadBufferTemp, mReadBufferTemp.Length);
            }
            else
            {
                LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:ws://" + mNetHoster.ToString());
                DisConnect();
            }
        }

        private void OnDataReceivedCallback(object sender, DataReceivedEventArgs de)
        {
            mReadBufferTemp = de.Data;
            if (mReadBufferTemp.Length > 0)
            {
                mReadBuffer.Push(mReadBufferTemp, mReadBufferTemp.Length);
            }
            else
            {
                LoggerSystem.Instance.Error("读取数据为0，将要断开此链接接:ws://" + mNetHoster.ToString());
                DisConnect();
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
            int length = mSendBuffer.DataSize();
            if (IsConnected() && length > 0 && mSocket.State == WebSocketState.Open)
            {
                mSocket.Send(mSendBuffer.Buffer(), 0, length);
                mSendBuffer.Pop(length);
            }
        }

    }
}
