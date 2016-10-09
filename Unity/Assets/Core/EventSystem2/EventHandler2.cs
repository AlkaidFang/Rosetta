using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
	public class EventHandler2
	{
		private int mKey;
		private object mHoster;
		private object mData;
		private Callback<int, object, object[]> mHandler;

		private int mFireCount;

		public EventHandler2()
		{
			mKey = -1;
			mHoster = null;
			mData = null;
			mHandler = null;
			mFireCount = 0;
		}

		public void Init(int key, object hoster, object data, Callback<int, object, object[]> handler)
		{
			mKey = key;
			mHoster = hoster;
			mData = data;
			mHandler = handler;
		}

		public void test ()
		{

		}

		public void Fire(Event2 e)
		{
			if (mHandler != null && mHoster != null)
			{
				mHandler (e.GetKey(), mData, e.GetArgs ());
				mFireCount++;
			}
			else
			{
				LoggerSystem.Instance.Error("EventLisener Error! hoster:" + this.mHoster + ", handler:" + this.mHandler);
			}
		}

		public bool IsEvent(int key, object hoster)
		{
			return this.mKey == key && this.mHoster == hoster;
		}
	}
}
