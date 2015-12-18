using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alkaid
{
    public class INetConnector
    {
        public static int MAX_SOCKET_BUFFER_SIZE = 4096;

        public enum ConnectionType
        {
            TYPE_TCP = 1,
            TYPE_UDP = 2,
            TYPE_WSOCKT = 3,
            TYPE_HTTP = 4,
            TYPE_UNKNOW = 5,
        }

        public Callback OnConnected;
        public Callback<MemoryStream> OnRecieved;
        public Callback OnDisconnected;
        public Callback OnError;

        protected NetHost mNetHoster;
        protected bool mIsConnected;

        public INetConnector()
        {
            mNetHoster = null;
            OnConnected = null;
            OnRecieved = null;
            OnDisconnected = null;
            OnError = null;
        }

        public virtual ConnectionType GetConnectionType()
        {
            return ConnectionType.TYPE_UNKNOW;
        }

        public virtual bool Connect(string address, int port)
        {
            mNetHoster = new NetHost(address, port);
            return false;
        }

        public virtual void DisConnect()
        {

        }

        protected void CallbackConnected()
        {
            if (OnConnected != null)
            {
                OnConnected();
            }
        }

        protected void CallbackRecieved(MemoryStream ms)
        {
            if (OnRecieved != null && ms != null)
            {
                OnRecieved(ms);
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
    }
}
