﻿using System;

// 最终包格式为：
//         cc(2byte) + length(4byte) + type(2byte) + data

namespace Alkaid
{
    public interface IPacketFormat
    {
        //static byte[] PACKET_HEAD = { 99, 99 }; //{'c', 'c'};

        int GetLength(int dataLength);

        // 组装这个包
        void GenerateBuffer(ref Byte[] dest, IPacket packet);

        //  检查当前缓冲区中是否包含一个包
        bool CheckHavePacket(Byte[] buffer, int offset);

        // 解码这个包
        bool DecodePacket(Byte[] buffer, ref int packetLength, ref int packetType, ref Byte[] data);
    }
}