using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class SCHelloWorldResultHandler : IPacketHandler
    {
        public int GetPacketType()
        {
            return (int)PacketType.SC_HelloWorldResult;
        }

        public bool OnPacketHandler(object proto)
        {
            //Console.WriteLine("CSHelloWorldHandler  OnPacketHandler");
            XMessage.SC_HelloWorldResult data = proto as XMessage.SC_HelloWorldResult;
            LoggerSystem.Instance.Info("收到回复:" + data._resultCode);

            // Console.Out.WriteLine("收到包：" + packet.a + "  " + packet.b + "  " + packet.c + "  " + packet.d + "    =====" + (++i));

            return true;
        }
    }
}
