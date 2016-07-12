using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Alkaid
{
    /**
     * 用于存储一系列非共同基类派生的对象的容器
     * 类似于C++的union集合的概念
     * 注意：
     *      1，每种类型在此容器中只有一个实例
     *      2，hashtable和运行时类型信息会导致这个类的性能不高
     * */

    public class UnionDataCollection
    {
        private Hashtable mCollection;

        public UnionDataCollection()
        {
            mCollection = new Hashtable();
        }

        public T Get<T>()
        {
            T ret = default(T);
            Type key = typeof(T);
            if (mCollection.ContainsKey(key))
            {
                ret = (T)mCollection[key];
            }

            return ret;
        }

        public void Add<T>(T v)
        {
            if (v == null) return;

            Type key = v.GetType();
            if (mCollection.ContainsKey(key))
            {
                mCollection[key] = v;
            }
            else
            {
                mCollection.Add(key, v);
            }
        }

        public void Remove<T>(T v)
        {
            Type key = typeof(T);
            if (mCollection.ContainsKey(key))
            {
                RemoveType<T>();
            }
        }

        public void RemoveType<T>()
        {
            Type key = typeof(T);
            if (mCollection.ContainsKey(key))
            {
                mCollection.Remove(key);
            }
        }

        public void Clear()
        {
            mCollection.Clear();
        }

        public void Foreach(Callback<object> cb)
        {
            foreach (var i in mCollection.Values)
            {
                cb(i);
            }
        }

        public T GetOrNew<T>() where T : new()
        {
            T ret = Get<T>();
            if (null == ret)
            {
                ret = new T();
                Add(ret);
            }

            return ret;
        }


    }
}
