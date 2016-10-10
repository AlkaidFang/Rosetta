using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
	public class Event2 : IPoolObject<Event2>
	{
		private int mKey;
		private object[] mArgs;

		public void Set(int key, object[] args)
		{
			mKey = key;
			mArgs = args;
		}

		public int GetKey()
		{
			return mKey;
		}

		public  object[] GetArgs()
		{
			return mArgs;
		}
	}
}
