using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Event
    {
        private string mKey;
        private object[] mArgs;
        private int mDelayTicks;


        public Event(string key, object[] args)
        {
            mKey = key;
            mArgs = args;
            mDelayTicks = 0;
        }

        public Event(string key, object[] args, int delayTicks)
        {
            mKey = key;
            mArgs = args;
            mDelayTicks = delayTicks;
        }

        public void DoTick()
        {
            mDelayTicks--;
        }

        public bool CanFire()
        {
            return mDelayTicks == 0;
        }

        public string GetKey()
        {
            return mKey;
        }

        public  object[] GetArgs()
        {
            return mArgs;
        }
    }
}
