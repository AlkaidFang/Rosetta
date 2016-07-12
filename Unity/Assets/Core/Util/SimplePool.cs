using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class IPoolObject<T> where T : IPoolObject<T>, new ()
    {
        private SimplePool<T> mAssociatedPool;

        public IPoolObject()
        {
            mAssociatedPool = null;
        }

        public void InitPool(SimplePool<T> associatedPool)
        {
            mAssociatedPool = associatedPool;
        }

        protected void Recycle(T t)
        {
            mAssociatedPool.Recycle(t);
        }
    }

    public class SimplePool<T> where T : IPoolObject<T>, new()
    {
        private object mAsyncLocker;
        private int mPoolSize;
        private Queue<T> mFreeObjects;
        private List<T> mBusyObjects;

        public SimplePool()
        {
            mAsyncLocker = new object();
            mPoolSize = 0;
            mFreeObjects = new Queue<T>();
            mBusyObjects = new List<T>();
        }

        public void Init(int size)
        {
            for (int i = 0; i < mPoolSize; ++i)
            {
                mFreeObjects.Enqueue(NewOne());
            }
        }

        private T NewOne()
        {
            T t = new T();
            if (null != t)
            {
                ++mPoolSize;
                t.InitPool(this);
            }

            return t;
        }

        public T Alloc()
        {
            lock (GetLocker())
            {
                T ret = default(T);

                if (mFreeObjects.Count > 0)
                {
                    ret = mFreeObjects.Dequeue();
                }
                else
                {
                    ret = NewOne();
                }

                mBusyObjects.Add(ret);
                return ret;
            }

        }

        public void Recycle(T t)
        {
            lock(GetLocker())
            {
                if (null != t)
                {
                    mFreeObjects.Enqueue(t);

                    mBusyObjects.Remove(t);
                }
            }
        }

        public int GetSize()
        {
            return mPoolSize;
        }

        public int GetFreeCount()
        {
            return mFreeObjects.Count;
        }

        public object GetLocker()
        {
            return mAsyncLocker;
        }
    }
}
