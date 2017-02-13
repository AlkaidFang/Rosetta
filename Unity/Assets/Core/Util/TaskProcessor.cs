using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class TaskProcessor
    {
        private SafeQueue<Callback> mTaskMaskQueue;
        private UnionDataCollection mTaskTypePools;

        public TaskProcessor()
        {
            mTaskMaskQueue = new SafeQueue<Callback>();
            mTaskTypePools = new UnionDataCollection();
        }

        public void HandleTasks(int size)
        {
            Callback cb = null;
            for (int i = 0; i < size; ++i)
            {
                cb = mTaskMaskQueue.Dequeue();
                if (null != cb)
                {
                    cb();
                }
                else
                {
                    break;
                }
            }
        }

        public void Reset()
        {
            mTaskMaskQueue.Clear();
            mTaskTypePools.Clear();
        }

        public void PushExcute(Callback cb)
        {
            mTaskMaskQueue.Enqueue(cb);
        }

        public void PushExcute(Callback<List<object>> cb, params object[] args)
        {
            bool newTaskFailed = true;
            SimplePool<Task> pool = mTaskTypePools.GetOrNew<SimplePool<Task>>();
            if (null != pool)
            {
                Task task = pool.Alloc();
                if (null != task)
                {
                    task.Init(cb, args);
                    mTaskMaskQueue.Enqueue(task.Run);
                    newTaskFailed = false;
                }
            }
            if (newTaskFailed)
            {
                mTaskMaskQueue.Enqueue(() =>
                {
                    List<object> ps = new List<object>();
                    ps.AddRange(args);
                    cb(ps);
                });
            }
        }

        // Callback<params object[] args>
        public void PushExcute<T0>(Callback<T0> cb, T0 t)
        {
            bool newTaskFailed = true;
            SimplePool<Task<T0>> pool = mTaskTypePools.GetOrNew<SimplePool<Task<T0>>>();
            if (null != pool)
            {
                Task<T0> task = pool.Alloc();
                if (null != task)
                {
                    task.Init(cb, t);
                    mTaskMaskQueue.Enqueue(task.Run);
                    newTaskFailed = false;
                }
            }
            if (newTaskFailed)
            {
                mTaskMaskQueue.Enqueue(() =>
                {
                    cb(t);
                });
            }
        }


    }
}
