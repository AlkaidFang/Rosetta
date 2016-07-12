using System;
using System.Collections.Generic;

namespace Alkaid
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
            Google.Protobuf.IMessage im = (Google.Protobuf.IMessage)mProto;
            if (im != null)
            {
                mProtoBytes = Google.Protobuf.MessageExtensions.ToByteArray(im);
            }
        }
    }
}
