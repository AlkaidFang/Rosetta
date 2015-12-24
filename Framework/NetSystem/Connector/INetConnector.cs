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
        public Callback<int, MemoryStream> OnRecieved;
        public Callback OnDisconnected;
        public Callback OnError;

        private int mUid;
        protected NetHost mNetHoster;
        protected IPacketFormat mPacketFormat;
        protected IPacketHandlerManager mPacketHandlerManager;
        protected bool mIsConnected;

        public INetConnector(IPacketFormat packetFormat, IPacketHandlerManager packetHandlerManager)
        {
            mUid = -1;
            mNetHoster = null;
            OnConnected = null;
            OnRecieved = null;
            OnDisconnected = null;
            OnError = null;

            mPacketFormat = packetFormat;
            mPacketHandlerManager = packetHandlerManager;
        }

        public virtual bool Init()
        {
            return false;
        }

        public virtual void Tick(float interval)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual ConnectionType GetConnectionType()
        {
            return ConnectionType.UNKNOW;
        }

        public virtual bool Connect(string address, int port)
        {
            mNetHoster = new NetHost(address, port);
            return false;
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

        protected void CallbackRecieved(int ptype, MemoryStream ms)
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
                OnDisconnected();
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
    }
}
