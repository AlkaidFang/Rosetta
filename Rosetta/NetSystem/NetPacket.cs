using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class NetPacket : IPacket
    {
        private PacketType mType;
        private Byte[] mProtoBytes;
        private object mProto;

        public object Proto
        {
            set
            {
                mProto = value;
                EncodeProto();
            }
        }

        public NetPacket(PacketType type)
        {
            mType = type;
            mProtoBytes = null;
            mProto = null;
        }

        public int GetPacketType()
        {
            return (int)mType;
        }

        public Byte[] GetData()
        {
            return mProtoBytes;
        }

        private void EncodeProto()
        {
            if (mProto != null)
            {
                System.IO.MemoryStream Stream = new System.IO.MemoryStream();
                ProtoBuf.Serializer.Serialize(Stream, mProto);
                mProtoBytes = Stream.GetBuffer();
            }
        }
    }
}
