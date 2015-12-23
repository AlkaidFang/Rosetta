using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class PacketFormat : IPacketFormat
    {
        public static byte[] PACKET_HEAD = { 99, 99 }; //{'c', 'c'};

        public int GetLength(System.IO.MemoryStream data)
        {
            return 2 + 4 + 2 + (int)data.Length;
        }

        // 组装这个包
        public void GenerateBuffer(ref Byte[] dest, IPacket packet)
        {
            System.IO.MemoryStream data = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize(data, packet);
            int iLength = GetLength(data);

            dest = new Byte[iLength];

            // 包头
            Array.Copy(PACKET_HEAD, 0, dest, 0, 2);

            // 长度
            Byte[] bLength = BitConverter.GetBytes(iLength);
            Array.Copy(bLength, 0, dest, 2, 4);

            // 类型
            short iType = (short)packet.GetPacketType();
            Byte[] bType = BitConverter.GetBytes(iType);
            Array.Copy(bType, 0, dest, 6, 2);

            // 数据
            Array.Copy(data.GetBuffer(), 0, dest, 8, data.Length);
        }

        //  检查当前缓冲区中是否包含一个包
        public bool CheckHavePacket(Byte[] buffer, int offset)
        {
            if (buffer[0] == PACKET_HEAD[0] && buffer[1] == PACKET_HEAD[1]) // 首两位为包头
            {
                int length = BitConverter.ToInt32(buffer, 2);
                if (length <= offset)
                {
                    return true;
                }
            }

            return false;
        }

        // 解码这个包
        public bool DecodePacket(Byte[] buffer, ref int packetLength, ref int packetType, ref System.IO.MemoryStream proto)
        {
            do
            {
                packetLength = BitConverter.ToInt32(buffer, 2);
                if (packetLength < 0)
                    break;

                packetType = BitConverter.ToInt16(buffer, 6);
                if (packetType < 0)
                    break;

                if (packetType > 1)
                {
                    int i = 0;
                }

                proto = new System.IO.MemoryStream(buffer, 8, packetLength - 8);
                if (null == proto)
                    break;

                NetPacket packet = ProtoBuf.Serializer.Deserialize<NetPacket>(proto);
                proto = new System.IO.MemoryStream(packet.mProtoStream);

                return true;
            }
            while (false);

            return false;
        }
    }
}
