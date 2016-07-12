using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{

    public interface IPacket
    {

        int GetPacketType();

        Byte[] GetData();
    }
}
