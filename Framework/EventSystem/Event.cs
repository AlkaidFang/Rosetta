using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Event
    {
        private string mKey;
        private object mHoster;
        private Delegate mHandler;

        private int mFireCount;

        public Event()
        {
            mKey = "";
            mHoster = null;
            mHandler = null;
            mFireCount = 0;
        }

        public void Init(string key, object hoster, Delegate handler)
        {
            this.mKey = key;
            this.mHoster = hoster;
            this.mHandler = handler;

        }

        public void Fire(string key, params object[] parameters)
        {
            if (this.mHandler != null && this.mHoster != null)
            {
                this.mHandler.DynamicInvoke(parameters);
                this.mFireCount++;
            }
            else
            {
                LoggerSystem.Instance.Error("EventLisener Error! hoster:" + this.mHoster + ", handler:" + this.mHandler);
            }
        }

        public bool IsEvent(string key, object hoster)
        {
            return this.mKey == key && this.mHoster == hoster;
        }
    }
}
