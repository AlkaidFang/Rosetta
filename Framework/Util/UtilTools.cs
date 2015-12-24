using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
