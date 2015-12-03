using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class TimeSystem : Singleton<TimeSystem>, Lifecycle
    {

        private int mFrames;
        private double mTotalTickSeconds;
        private double mLocalStartTime;
        private float mRemoteTimeOffset;

        private DateTime _BaseTime;

        public TimeSystem()
        {
            _BaseTime = new DateTime(1970, 1, 1);

            mFrames = 0;
            mTotalTickSeconds = 0;
            mLocalStartTime = 0;
            mRemoteTimeOffset = 0;
        }

        public bool Init()
        {
            LoggerSystem.Instance.Info("TimeSystem    init  begin");
            this.mLocalStartTime = this.GetMillisecods();
            this.mTotalTickSeconds = 0;

            LoggerSystem.Instance.Info("TimeSystem    init  end");
            return true;
        }

        public void Tick(float interval)
        {
            LoggerSystem.Instance.Debug("每tick  时间s: " + interval);

            this.mFrames++;
            this.mTotalTickSeconds += interval;
        }

        public void Destroy()
        {
            LoggerSystem.Instance.Info("TimeSystem    destroy  begin");
            this.mFrames = 0;
            this.mTotalTickSeconds = 0;

            LoggerSystem.Instance.Info("TimeSystem    destroy  end");
        }

        public float GetRemoteTimeOffset()
        {
            return mRemoteTimeOffset;
        }

        public void SetRemoteTimeOffset(float timeOffset)
        {
            mRemoteTimeOffset = timeOffset;
        }

        public double GetMillisecods()
        {
            return DateTime.Now.Subtract(_BaseTime).TotalMilliseconds;
        }

        public double GetLocalMilliseconds()
        {
            return this.GetMillisecods() - this.mLocalStartTime;
        }

        public double GetServerMilliseconds()
        {
            return this.GetLocalMilliseconds() - this.mRemoteTimeOffset;
        }

        public void ResetFrame()
        {
            this.mFrames = 0;
        }

        public int GetFrame()
        {
            return mFrames;
        }

        public double GetRunTime()
        {
            return mTotalTickSeconds;
        }

        public DateTime GetClientTime()
        {
            return DateTime.Now;
        }

        /*
        public DateTime GetServerTime()
        {

        }
        */

        public int GetIntervalDay(double start, double end)
        {
            double intervalMilliseconds = end - start;
            int intervalDay = (int)(intervalMilliseconds / (1000 * 60 * 60 * 24));
            return intervalDay;
        }
    }
}
