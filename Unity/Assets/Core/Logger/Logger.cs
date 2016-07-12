using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Logger : Lifecycle
    {
        private Callback<string> mOutput = null;

        public Logger()
        {

        }

        public Logger(Callback<string> callback)
        {
            mOutput = callback;
        }

        /*protected void SetDelegate(Callback<string> callback)
        {
            mOutput = callback;
        }*/

        public virtual void Write(string message)
        {
            if (null != mOutput)
            {
                mOutput(message);
            }
        }

        public virtual bool Init()
        {
            return true;
        }

        public virtual void Tick(float interval)
        {

        }

        public virtual void Destroy()
        {

        }

    }
}
