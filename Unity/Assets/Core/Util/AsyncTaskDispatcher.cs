using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class AsyncTaskDispatcher
    {
        private const int ConstTaskThreadNum = 5;
        private bool mShareTask;
        private int mThreadCount;
        private int mIndexRnd;
        private List<AsyncThread> mThreadList;

        public AsyncTaskDispatcher()
        {
            mShareTask = false;
            mThreadCount = ConstTaskThreadNum;
            mIndexRnd = 0;
            mThreadList = new List<AsyncThread>();

            Init();
        }

        public AsyncTaskDispatcher(int threadNum, bool shareTask)
        {
            mThreadCount = threadNum;
            mShareTask = shareTask;
            mIndexRnd = 0;
            mThreadList = new List<AsyncThread>();

            Init();
        }

        public void Reset()
        {
            mShareTask = false;
            mThreadCount = ConstTaskThreadNum;
            mIndexRnd = 0;

			for (int i = 0; i < mThreadCount; ++i)
			{
				mThreadList [i].Stop ();
			}

            mThreadList.Clear();
        }

        private void Init()
        {
            TaskProcessor tp = null;
            if (mShareTask)
            {
                tp = new TaskProcessor();
            }

            for (int i = 0; i < mThreadCount; ++i)
            {
                if (!mShareTask)
                {
                    tp = new TaskProcessor();
                }
                mThreadList.Add(new AsyncThread(Process, tp));

				mThreadList [i].Start ();
            }
        }

        private void Process(AsyncThread thread)
        {
            while (thread.IsWorking())
            {
                ((TaskProcessor)thread.GetExtraData()).HandleTasks(50);

                System.Threading.Thread.Sleep(10);
            }
        }

        public TaskProcessor GetProcessor()
        {
            AsyncThread at = mThreadList[(mIndexRnd++) % mThreadCount];
            return (TaskProcessor)at.GetExtraData();
        }

    }
}