using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
	public class Event2
	{
		private int mKey;
		private object[] mArgs;

		public Event2(int key, object[] args)
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
