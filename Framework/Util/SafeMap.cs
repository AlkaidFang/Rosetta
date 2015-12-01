using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class SafeMap<K, V>
    {
        private Dictionary<K, V> handlerDict;

        private Dictionary<K, V> tempDict;

        private volatile int hasChanged;

        public Dictionary<K, V>.KeyCollection Keys
        {
            get { return handlerDict.Keys; }
        }

        public int Count
        {
            get { return handlerDict.Count; }
        }

        public SafeMap()
        {
            handlerDict = new Dictionary<K, V>();
            tempDict = new Dictionary<K, V>();
            hasChanged = 0;
        }

        public void Put(K key, V value)
        {
            lock (handlerDict)
            {
                handlerDict.Add(key, value);

                ++hasChanged;
            }
        }

        public V Get(K key)
        {
            lock (handlerDict)
            {
                return handlerDict[key];
            }
        }

        public void Remove(K key)
        {
            lock (handlerDict)
            {
                handlerDict.Remove(key);

                ++hasChanged;
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
            lock (handlerDict)
            {
                handlerDict.Clear();
            }
        }

        public delegate void _foreach_delegate_(V arg);
        public void Foreach(_foreach_delegate_ _delegate)
        {
            // foreach 此处使用他的缓存副本
            if (hasChanged > 0)
            {
                tempDict.Clear();
                lock (handlerDict)
                {
                    foreach (var item in handlerDict)
                    {
                        tempDict.Add(item.Key, item.Value);
                    }
                }

                if (--hasChanged > 0)
                {
                    hasChanged = 1;
                }
            }

            lock (tempDict)
            {
                foreach (K key in tempDict.Keys)
                {
                    _delegate(tempDict[key]);
                }
            }

        }
    }
}
