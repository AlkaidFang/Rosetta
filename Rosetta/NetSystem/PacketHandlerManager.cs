using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class PacketHandlerManager : IPacketHandlerManager
    {
        private class PacketHandlerInfo
        {
            public Type mProtoType;
            public IPacketHandler mHandler;

            public PacketHandlerInfo(Type t, IPacketHandler h)
            {
                mProtoType = t;
                mHandler = h;
            }
        }

        private Dictionary<int, PacketHandlerInfo> mHandlerDict = null;

        public PacketHandlerManager()
        {
            mHandlerDict = new Dictionary<int, PacketHandlerInfo>();
        }

        ~PacketHandlerManager()
        {

        }

        public bool Init()
        {
            return true;
        }

        public void Tick(float interval)
        {
            // don't need
        }

        public void Destroy()
        {
            mHandlerDict.Clear();
        }


        public void RegisterHandler(Type protoType, IPacketHandler handler)
        {
            if (null != handler)
            {
                mHandlerDict.Add(handler.GetPacketType(), new PacketHandlerInfo(protoType, handler));
            }
        }

        public bool DispatchHandler(int type, Byte[] data)
        {
            if (data != null && mHandlerDict.ContainsKey(type))
            {
                PacketHandlerInfo handlerInfo = mHandlerDict[type];
                if (null != handlerInfo)
                {
                    return handlerInfo.mHandler.OnPacketHandler(data);
                }
            }

            return false;
        }

    }
}
