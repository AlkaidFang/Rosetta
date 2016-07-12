using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    class SafeList<T>
    {
        private object mLocker;
        private List<T> mValueList;
        private List<T> mTempList;
        private volatile int mHaveChanged;

        public int Count
        {
            get { return mValueList.Count; }
        }

        public SafeList()
        {
            mLocker = new object();
            mValueList = new List<T>();
            mTempList = new List<T>();
            mHaveChanged = 0;
        }

        private void _Reset()
        {
            mValueList.Clear();
            mTempList.Clear();
            mHaveChanged = 0;
        }

        public void Add(T value)
        {
            lock (mLocker)
            {
                mValueList.Add(value);
                ++mHaveChanged;
            }
        }

        public void Remove(T value)
        {
            lock (mLocker)
            {
                mValueList.Remove(value);
                ++mHaveChanged;
            }
        }

        public T Find(Predicate<T> match)
        {
            lock (mLocker)
            {
                return mValueList.Find(match);
            }
        }

        public void Clear()
        {
            lock (mLocker)
            {
                _Reset();
            }
        }

        public void Foreach(Callback<T> _delegate)
        {
            lock (mLocker)
            {
                if (mHaveChanged > 0)
                {
                    mTempList.Clear();
                    foreach (var i in mValueList)
                    {
                        mTempList.Add(i);
                    }

                    if (--mHaveChanged > 0)
                    {
                        mHaveChanged = 1;
                    }
                }

                foreach (var i in mTempList)
                {
                    _delegate(i);
                }
            }
        }

        /*
         * 不要优先考虑这个方法，这个方法将内部数据给外部，并不太好
         * */
        public void Lock(Callback<List<T>> _delegate)
        {
            lock (mLocker)
            {
                _delegate(mValueList);
            }
        }
    }
}
