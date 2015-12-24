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

        public void RegisterConnector(int uid, ConnectionType type, IPacketFormat pf, IPacketHandlerManager phm, Callback<bool> connected, Callback<int, System.IO.MemoryStream> recieved, Callback disconnected, Callback error)
        {
            INetConnector ctor = null;
            switch (type)
            {
                case ConnectionType.TCP: ctor = new TCPConnector(pf, phm); break;
                case ConnectionType.UDP: ctor = new UDPConnector(pf, phm); break;

                default: ctor = new TCPConnector(pf, phm); break;
            }

            ctor.OnConnected = connected;
            ctor.OnRecieved = recieved;
            ctor.OnDisconnected = disconnected;
            ctor.OnError = error;
            ctor.SetUid(uid);

            mConnectorMap.Add(uid, ctor);
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
