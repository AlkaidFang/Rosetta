using System;
using System.Threading;

namespace Alkaid
{

    public class AsyncThreading
    {
        public enum ThreadStatus
        {
            None,
            Start,
            Working,
            Stop,
            Finished,
        }

        private volatile ThreadStatus mStatus; // cause there only one method to change this value, no need to lock.
        private Callback<AsyncThreading> mContext;
        private Callback mFinishedContext;
        private Thread mThread;

        public AsyncThreading(Callback<AsyncThreading> cb)
        {
            mStatus = ThreadStatus.None;
            mContext = cb;
            mThread = new Thread(new ThreadStart(ContextMask));
        }

        /**
         * This mask function may cause a little perfermance problem.
         * */
        private void ContextMask()
        {
            mStatus = ThreadStatus.Working;
            mContext(this);

            if (mStatus == ThreadStatus.Stop)
            {
                if (null != mFinishedContext)
                {
                    mFinishedContext();
                }

                mStatus = ThreadStatus.Finished;
            }
        }

        private void Release()
        {
            mContext = null;
            mFinishedContext = null;
            mThread = null;
        }

        public bool Start()
        {
            if (null != mThread)
            {
                mStatus = ThreadStatus.Start;
                mThread.Start();

                return true;
            }

            return false;
        }

        public void Stop()
        {
            if (IsWorking())
            {
                mFinishedContext = null;
                mStatus = ThreadStatus.Stop;
            }
        }

        /**
         * cb will be called in async-thread
         * */
        public void AsyncStop(Callback cb)
        {
            if (IsWorking())
            {
                mFinishedContext = cb;
                mStatus = ThreadStatus.Stop;
            }
        }

        /**
         * cb will be called in sync-thread
         * */
        public void SyncStop(Callback cb)
        {
            if (IsWorking())
            {
                mFinishedContext = null;
                mStatus = ThreadStatus.Stop;

                while (mStatus != ThreadStatus.Finished) ;
                cb();
            }
            
        }

        public bool IsWorking()
        {
            return mStatus == ThreadStatus.Working;
        }

        public int GetThreadId()
        {
            return null == mThread ? -1 : mThread.ManagedThreadId;
        }
    }
}
