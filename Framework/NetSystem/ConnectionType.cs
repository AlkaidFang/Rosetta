using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public enum ConnectionType : int
    {
        TCP = 1,
        UDP = 2,
        WSOCKT = 3,
        HTTP = 4,
        UNKNOW = 5,
    }
}
