using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class SafeMap<K, V>
    {
        private object mLocker;
        private Dictionary<K, V> mValueDict;
        private Dictionary<K, V> mTempDict;
        private volatile int mHaveChanged;

        public Dictionary<K, V>.KeyCollection Keys
        {
            get { return mValueDict.Keys; }
        }

        public int Count
        {
            get { return mValueDict.Count; }
        }

        public SafeMap()
        {
            mLocker = new object();
            mValueDict = new Dictionary<K, V>();
            mTempDict = new Dictionary<K, V>();
            mHaveChanged = 0;
        }

        public void Put(K key, V value)
        {
            lock (mLocker)
            {
                mValueDict.Add(key, value);

                ++mHaveChanged;
            }
        }

        public V Get(K key)
        {
            lock (mLocker)
            {
                return mValueDict[key];
            }
        }

        public void Remove(K key)
        {
            lock (mLocker)
            {
                mValueDict.Remove(key);

                ++mHaveChanged;
            }
        }

        //public List<T> ToList()
        //{
        //    lock (handlerList)
        //    {
        //        return handlerList.ToList();
        //    }
        //}

        public void Clear()
        {
            lock (mLocker)
            {
                mValueDict.Clear();
            }
        }

        public void Foreach(Callback<V> _delegate)
        {
            // foreach 此处使用他的缓存副本
            if (mHaveChanged > 0)
            {
                mTempDict.Clear();
                lock (mLocker)
                {
                    foreach (var item in mValueDict)
                    {
                        mTempDict.Add(item.Key, item.Value);
                    }
                }

                if (--mHaveChanged > 0)
                {
                    mHaveChanged = 1;
                }
            }

            lock (mTempDict)
            {
                foreach (K key in mTempDict.Keys)
                {
                    _delegate(mTempDict[key]);
                }
            }

        }
    }
}
