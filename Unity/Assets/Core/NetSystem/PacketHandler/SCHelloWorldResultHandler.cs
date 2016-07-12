using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class SCHelloWorldResultHandler : IPacketHandler
    {
        public int GetPacketType()
        {
            return (int)PacketType.SC_HelloWorldResult;
        }

        public bool OnPacketHandler(Byte[] proto)
        {
            //Console.WriteLine("CSHelloWorldHandler  OnPacketHandler");
            XMessage.SC_HelloWorldResult data = XMessage.SC_HelloWorldResult.Parser.ParseFrom(proto);
            LoggerSystem.Instance.Info("收到回复:" + data.ResultCode);

            // Console.Out.WriteLine("收到包：" + packet.a + "  " + packet.b + "  " + packet.c + "  " + packet.d + "    =====" + (++i));

            return true;
        }
    }
}
