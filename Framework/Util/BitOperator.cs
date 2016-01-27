using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class BitOperator
    {
        private const int BYTESIZE = 8;
        public static bool GetBitFlag(byte[] data, int index)
        {
            if (null == data)
            {
                return false;
            }

            if (index < 0 || index >= data.Length * BYTESIZE)
            {
                return false;
            }

            byte b = (byte)(1 << (index % BYTESIZE));

            return (b & data[index /BYTESIZE]) == b;
        }

        public static bool GetBitFlag(int data, int index)
        {
            if (index < 0 || index >= sizeof(int) * BYTESIZE)
            {
                return false;
            }

            int num = 1 << index;

            return (num & data) == num;
        }

        public static bool SetBitFlag(byte[] data, int index, bool flag)
        {
            if (null == data)
            {
                return false;
            }
            
            if (index < 0 || index >= data.Length * BYTESIZE)
            {
                return false;
            }

            byte b =  (byte)(1 << (index % BYTESIZE));
            if (flag)
            {
                data[index / BYTESIZE] |= b;
            }
            else
            {
                data[index / BYTESIZE] &= (byte)(~b);
            }

            return true;
        }
    }
}
