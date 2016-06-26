using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class EventHandler
    {
        private string mKey;
        private object mHoster;
		private Delegate mHandler;

        private int mFireCount;

        public EventHandler()
        {
            mKey = string.Empty;
            mHoster = null;
            mHandler = null;
            mFireCount = 0;
        }

		public void Init(string key, object hoster, Delegate handler)
        {
            mKey = key;
            mHoster = hoster;
            mHandler = handler;

        }

		public void test ()
		{
			
		}

        public void Fire(string key, object[] parameters)
        {
            if (mHandler != null && mHoster != null)
            {
                mHandler.DynamicInvoke(parameters);
				//this.mHandler.Invoke (parameters);
				//this.mHandler (parameters);
                mFireCount++;
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
