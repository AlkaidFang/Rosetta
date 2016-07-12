using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class PacketFormat : IPacketFormat
    {
        public static byte[] PACKET_HEAD = { 99, 99 }; //{'c', 'c'};

        public int GetLength(int dataLength)
        {
            return 2 + 4 + 4 + dataLength;
        }

        // 组装这个包
        public void GenerateBuffer(ref Byte[] dest, IPacket packet)
        {
            Byte[] data = packet.GetData();
            int iLength = GetLength(data.Length);

            dest = new Byte[iLength];

            // 包头
            Array.Copy(PACKET_HEAD, 0, dest, 0, 2);

            // 长度
            Byte[] bLength = BitConverter.GetBytes(iLength);
            Array.Copy(bLength, 0, dest, 2, 4);

            // 类型
            int iType = packet.GetPacketType();
            Byte[] bType = BitConverter.GetBytes(iType);
            Array.Copy(bType, 0, dest, 6, 4);

            // 数据
            Array.Copy(data, 0, dest, 10, data.Length);
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
        public bool DecodePacket(Byte[] buffer, ref int packetLength, ref int packetType, ref Byte[] proto)
        {
            do
            {
                packetLength = BitConverter.ToInt32(buffer, 2);
                if (packetLength < 0)
                    break;

                packetType = BitConverter.ToInt32(buffer, 6);
                if (packetType < 0)
                    break;

                proto = new Byte[packetLength - 10];
                Array.Copy(buffer, 10, proto, 0, packetLength - 10);
                //proto = new System.IO.MemoryStream(buffer, 10, packetLength - 10);
                if (null == proto)
                    break;

                return true;
            }
            while (false);

            return false;
        }
    }
}
