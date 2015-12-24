using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class NetHost
    {
        private string mAddress;
        private int mPort;
        public NetHost(string address, int port)
        {
            mAddress = address;
            mPort = port;
        }

        public string GetAddress()
        {
            return mAddress;
        }

        public int GetPort()
        {
            return mPort;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", mAddress, mPort);
        }

    }
}
