using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public interface IPacketHandler
    {
        int GetPacketType();

        bool OnPacketHandler(object data);
    }
}
