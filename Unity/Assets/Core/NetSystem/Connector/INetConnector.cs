using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alkaid
{
    public class INetConnector: Lifecycle
    {
        public static int MAX_SOCKET_BUFFER_SIZE = 4096;

        public Callback<bool> OnConnected;
        public Callback<int, Byte[]> OnRecieved;
        public Callback OnDisconnected;
        public Callback OnError;

        private int mUid;
        protected RemoteHost mRemoteHost;
        protected IPacketFormat mPacketFormat;
        protected IPacketHandlerManager mPacketHandlerManager;

		private ConnectionStatus mConnectedStatus = ConnectionStatus.UNKNOW;

		private bool mDisconnectEvent; // 断开消息，必须要在主线程中处理

        public INetConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager)
        {
            mUid = -1;
            mRemoteHost = null;
            OnConnected = null;
            OnRecieved = null;
            OnDisconnected = null;
            OnError = null;

            mPacketFormat = packetFormat;
            mPacketHandlerManager = packetHandlerManager;

			mDisconnectEvent = false;
        }

        public virtual bool Init()
        {
            return mPacketHandlerManager.Init();
        }

        public virtual void Tick(float interval)
        {
            mPacketHandlerManager.Tick(interval);

			if (mDisconnectEvent) {
				mDisconnectEvent = false;
				OnDisconnected();
			}
        }

        public virtual void Destroy()
        {
            mPacketHandlerManager.Destroy();
        }

        public virtual ConnectionType GetConnectionType()
        {
            return ConnectionType.UNKNOW;
        }

        public virtual void Connect(string address, int port)
        {
            mRemoteHost = new RemoteHost(address, port);
            // return false;
        }

        public virtual void SendPacket(IPacket packet)
        {

        }

        public virtual void DisConnect()
        {

        }

        protected void CallbackConnected(bool status)
        {
            if (OnConnected != null)
            {
                OnConnected(status);
            }
        }

        protected void CallbackRecieved(int ptype, Byte[] ms)
        {
            if (OnRecieved != null && ms != null)
            {
                OnRecieved(ptype, ms);
            }
        }

        protected void CallbackDisconnected()
        {
			if (OnDisconnected != null)
			{
				mDisconnectEvent = true;
				//OnDisconnected();
			}
        }

        protected void CallbackError()
        {
            if (OnError != null)
            {
                OnError();
            }
        }

        public void SetUid(int id)
        {
            mUid = id;
        }

        public int GetUid()
        {
            return mUid;
        }

        public bool IsConnected()
        {
			return mConnectedStatus == ConnectionStatus.CONNECTED;
        }

		public ConnectionStatus GetConnectStatus()
		{
			return mConnectedStatus;
		}

		public void SetConnectStatus(ConnectionStatus status)
		{
			mConnectedStatus = status;
		}

    }
}
