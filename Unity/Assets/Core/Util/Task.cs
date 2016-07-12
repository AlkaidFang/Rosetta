using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Task : IPoolObject<Task>
    {
        private Callback<List<object>> mDelegate;
        private List<object> mParams;

        public Task()
        {
            mParams = new List<object>();
            
            Reset();
        }

        public void Init(Callback<List<object>> cb, params object[] args)
        {
            mDelegate = cb;
            mParams.AddRange(args);
        }

        public void Run()
        {
            mDelegate(mParams);
            
            Reset();

            Recycle(this);
        }

        public void Reset()
        {
            mDelegate = null;
            mParams.Clear();
        }

    }

    // Callback<params object[] args>
    public class Task<T0> : IPoolObject<Task<T0>>
    {
        private Callback<T0> mDelegate;
        private T0 mParams;

        public Task()
        {
            Reset();
        }

        public void Init(Callback<T0> cb, T0 args)
        {
            mDelegate = cb;
            mParams = args;
        }

        public void Run()
        {
            mDelegate(mParams);

            Reset();

            Recycle(this);
        }

        public void Reset()
        {
            mDelegate = null;
            mParams = default(T0);
        }
    }
}
