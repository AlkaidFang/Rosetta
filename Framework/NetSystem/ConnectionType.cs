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
        WEBSOCKET = 3,
        HTTP = 4,
        UNKNOW = 5,
    }

	public enum ConnectionStatus : int
	{
		UNKNOW = 0,
		INIT = 1,
		CONNECTING = 2,
		CONNECTED = 3,
		DISCONNECTED = 4,
		ERROR = 5,
	}

}
