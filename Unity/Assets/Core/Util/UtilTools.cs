using System;
using System.Collections.Generic;
using System.Text;

namespace Alkaid
{
    public class UtilTools
    {
        public static string GetCallStack()
        {
            System.Diagnostics.StackFrame[] stacks = new System.Diagnostics.StackTrace().GetFrames();
            string result = string.Empty;
            foreach (System.Diagnostics.StackFrame stack in stacks)
            {
                result += string.Format("File:{0}, Line:{1}, Col:{2}, Method:{3}\r\n", stack.GetFileName(),
                    stack.GetFileLineNumber(),
                    stack.GetFileColumnNumber(),
                    stack.GetMethod().ToString());
            }
            return result;
        }

        public static string BinToHex(byte[] data)
        {
            return BinToHex(data, 0, data.Length);
        }

        public static string BinToHex(byte[] data, int start, int length)
        {
            string ret = "";
            if (start < 0 || length <= 0 || start + length > data.Length)
                return ret;

            StringBuilder sb = new StringBuilder(length * 4);
            for (int i = 0; i < length; ++i)
            {
                sb.AppendFormat("{0,2:X2}", data[start + i]);
                if ((i + 1) % 16 == 0) sb.AppendLine();
                else sb.Append(' ');
            }

            ret = sb.ToString();
            return ret;
        }

        public static bool StringIsNullOrEmpty(string str)
        {
            str = str.ToLowerInvariant();
            str = str.Trim();
            if (str == null || str == "" || str == "None" || str == "null")
            {
                return true;
            }

            return false;
        }

        public static void Xor(ref byte[] buffer, byte[] xor)
        {
            int xorLen = xor.Length;
            int xorPos = 0;
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)(buffer[i] ^ xor[xorPos]);
                xorPos = (xorPos + 1) % xorLen;
            }
        }

    }
}
