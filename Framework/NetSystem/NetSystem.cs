using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class NetSystem : Singleton<NetSystem>, Lifecycle
    {
        /**
         * 本类进行实例化正式的网络连接模型
         * 提供各种模型的选择
         * 1，tcp
         * 2，udp
         * 3，websocket
         * */

        public enum NetType
        {
            TCP = 1,
            UDP,
            WEBSOCKET,
        }

        //private IPacketFormat mPacketFormat; // put it in connector, we suport multiple packetformat and handlermanager
        //private IPacketHandlerManager mPackerHandlerManager;
        private Dictionary<int, INetConnector> mConnectorMap;

        public NetSystem()
        {
            //mPacketFormat = null;
            //mPackerHandlerManager = null;
            mConnectorMap = new Dictionary<int, INetConnector>();
        }

        public bool Init()
        {
            foreach (var i in mConnectorMap.Values)
            {
                i.Init();
            }

            return true;
        }

        public void Tick(float interval)
        {

            foreach (var i in mConnectorMap.Values)
            {
                i.Tick(interval);
            }
        }

        public void Destroy()
        {

            foreach (var i in mConnectorMap.Values)
            {
                i.Destroy();
            }
        }

        public bool DispatchHandler(int type, System.IO.MemoryStream data)
        {

            return true;
        }

        public void RegisterConnector(int uid, NetType type, IPacketFormat pf, Callback<bool> connected = null, Callback<int, System.IO.MemoryStream> recieved = null, Callback disconnected = null, Callback error = null)
        {
            INetConnector connector = null;
            switch (type)
            {
                case NetType.TCP: connector = new TCPConnector(pf); break;

                default: connector = new TCPConnector(pf); break;
            }

            connector.OnConnected = connected;
            connector.OnRecieved = recieved;
            connector.OnDisconnected = disconnected;
            connector.OnError = error;

            mConnectorMap.Add(uid, connector);
        }

        public void Connect(int uid, string address, int port)
        {
            if (mConnectorMap.ContainsKey(uid))
            {
                mConnectorMap[uid].Connect(address, port);
            }
        }

        public void Send(int uid, IPacket packet)
        {
            if (mConnectorMap.ContainsKey(uid))
            {
                mConnectorMap[uid].SendPacket(packet);
            }
        }

        public void Close(int uid)
        {
            if (mConnectorMap.ContainsKey(uid))
            {
                mConnectorMap[uid].DisConnect();
            }
        }
    }
}
