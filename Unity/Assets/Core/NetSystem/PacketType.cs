using System;
using System.Collections.Generic;

namespace Alkaid
{
    public enum PacketType : int
    {
        CS_HelloWorld = 1,
        SC_HelloWorldResult,
        CS_Login,
        SC_LoginResult,
        CS_Ping,
        SC_PingResult,


        Size,
    }
}
