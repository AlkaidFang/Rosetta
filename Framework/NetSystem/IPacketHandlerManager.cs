using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public interface IPacketHandlerManager : Lifecycle
    {
        void RegisterHandler(Type protoType, IPacketHandler handler);

        bool DispatchHandler(int type, System.IO.MemoryStream data);
    }
}
