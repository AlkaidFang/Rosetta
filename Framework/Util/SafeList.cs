using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    class SafeList<T>
    {
        private List<T> mValueList;
        private List<T> mTempList;
        private volatile int mHaveChanged;

        public int Count
        {
            get { return mValueList.Count; }
        }

        public SafeList()
        {
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
            lock (mValueList)
            {
                mValueList.Add(value);
                ++mHaveChanged;
            }
        }

        public void Remove(T value)
        {
            lock (mValueList)
            {
                mValueList.Remove(value);
                ++mHaveChanged;
            }
        }

        public List<T> ToList()
        {
            lock (mValueList)
            {
                return mValueList.ToList();
            }
        }

        public T Find(Predicate<T> match)
        {
            lock (mValueList)
            {
                return mValueList.Find(match);
            }
        }

        public void Clear()
        {
            lock (mValueList)
            {
                _Reset();
            }
        }

        public void Foreach(Callback<T> _delegate)
        {
            if (mHaveChanged > 0)
            {
                mTempList.Clear();
                lock (mValueList)
                {
                    foreach (var i in mValueList)
                    {
                        mTempList.Add(i);
                    }
                }

                if (--mHaveChanged > 0)
                {
                    mHaveChanged = 1;
                }
            }

            lock(mTempList)
            {
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
            lock (mValueList)
            {
                _delegate(mValueList);
            }
        }
    }
}
