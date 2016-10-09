using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class SCPingResultHandler : IPacketHandler
    {
        public int GetPacketType()
        {
            return (int)PacketType.SC_PingResult;
        }

        public bool OnPacketHandler(Byte[] data)
        {
            XMessage.SC_PingResult proto = XMessage.SC_PingResult.Parser.ParseFrom(data);
            LoggerSystem.Instance.Info("收到回复:" + proto.Timestamp);

            DateTime d = DateTime.FromBinary((long)proto.Timestamp);
            double ms = (DateTime.Now - d).TotalMilliseconds;

            EventSystem.Instance.FireEvent("ping", "testwindow", ms);
			EventSystem2.Instance.FireEvent ((int)EventId.Ping, ms);

            return true;
        }
    }
}
