using System;
using System.Collections.Generic;
using Alkaid;
using ProtoBuf;

namespace Rosetta
{
    [ProtoContract]
    public class NetPacket : IPacket
    {
        [ProtoMember(1)]
        public PacketType mType;
        [ProtoMember(2)]
        public Byte[] mProtoStream;

        public object mProto;

        public NetPacket(PacketType type)
        {
            mType = type;
            mProtoStream = null;
            mProto = null;
        }

        public int GetPacketType()
        {
            return (int)mType;
        }

        public void EncodeProto()
        {
            if (mProto != null)
            {
                System.IO.MemoryStream Stream = new System.IO.MemoryStream();
                ProtoBuf.Serializer.Serialize(Stream, mProto);
                mProtoStream = Stream.GetBuffer();
            }
        }
    }
}
