using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class SafeQueue<T>
    {
        private Queue<T> queue = null;
        private object syncRoot = null;
        internal object SyncRoot
        {
            get { return syncRoot; }
        }

        public SafeQueue()
        {
            syncRoot = new object();
            queue = new Queue<T>();
        }

        public SafeQueue(IEnumerable<T> collection)
        {
            syncRoot = new object();
            queue = new Queue<T>(collection);
        }

        public SafeQueue(int capacity)
        {
            syncRoot = new object();
            queue = new Queue<T>(capacity);
        }

        public void Enqueue(T t)
        {
            lock(SyncRoot)
            {
                queue.Enqueue(t);
            }
        }

        public T Dequeue()
        {
            T ret = default(T);
            lock(SyncRoot)
            {
                ret = queue.Dequeue();
            }
            return ret;
        }

        public void Clear()
        {
            lock(SyncRoot)
            {
                queue.Clear();
            }
        }

        public bool Contains(T t)
        {
            bool ret = false;
            lock (SyncRoot)
            {
                ret = queue.Contains(t);
            }

            return ret;
        }

    }
}
