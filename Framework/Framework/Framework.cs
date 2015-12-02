using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alkaid
{
    public enum LogicThreadStatus
    {
        None,
        Start,
        Working,
        Stop,
        Finished,
    }

    public class Framework : Singleton<Framework>, Lifecycle
    {
        private Thread mLogicThread;
        private LogicThreadStatus mLogicThreadStatus;

        public Framework()
        {
            mLogicThread = null;
            mLogicThreadStatus = LogicThreadStatus.None;
        }

        public bool Init()
        {
            do
            {
                if (!LoggerSystem.Instance.Init()) break;
                LoggerSystem.Instance.Info("Framework init begin.");

                if (!DataProviderSystem.Instance.Init()) break;


                LoggerSystem.Instance.Info("Framework init end.");
                return true;
            }
            while(false);

            return false;
        }

        public void Tick()
        {
            LoggerSystem.Instance.Tick();
            DataProviderSystem.Instance.Tick();
        }

        public void Destroy()
        {
            LoggerSystem.Instance.Info("Framework destroy begin");

            LoggerSystem.Instance.Destroy();
            DataProviderSystem.Instance.Tick();

            LoggerSystem.Instance.Info("Framework destroy end.");

            LoggerSystem.Instance.Info("Good bye!");
        }

        private void LogicThreading()
        {
            LoggerSystem.Instance.Info("Logic Thread start.");

            int fps = FrameworkSetup.Instance.GetFPS();
            int sleepTime = 1000 / fps;
            LoggerSystem.Instance.Info("Logic Thread run at FPS:" + fps + ",  frame time is:" + sleepTime + "ms.");
            LoggerSystem.Instance.Info("Everything is ready, Let's play!");
            while (mLogicThreadStatus == LogicThreadStatus.Working)
            {
                System.DateTime d = System.DateTime.Now;

                Tick();

                System.Threading.Thread.Sleep(sleepTime);
            }

            LoggerSystem.Instance.Info("Logic Thread finished.");
            mLogicThreadStatus = LogicThreadStatus.Finished;
        }

        public void Start()
        {
            mLogicThreadStatus = LogicThreadStatus.Start;

            // setup
            FrameworkSetup.Instance.Apply();

            // Init
            bool ret = Init();
            if (!ret)
            {
                LoggerSystem.Instance.Fatal("Framework init failed.");
                return;
            }

            // Logic Thread start
            mLogicThread = new Thread(new ThreadStart(LogicThreading));
            if (mLogicThread != null)
            {
                mLogicThreadStatus = LogicThreadStatus.Working;
                mLogicThread.Start();
            }

        }

        public void Stop()
        {
            mLogicThreadStatus = LogicThreadStatus.Stop;

            while (mLogicThreadStatus != LogicThreadStatus.Finished) ;

            Destroy();
        }
    }








    public class MyRandom
    {
        public int GetRandomNum1000()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            return r.Next(1000);
        }

        public UnityEngine.GameObject CreateOne()
        {
            UnityEngine.GameObject go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
            go.transform.localPosition = UnityEngine.Vector3.zero;
            return go;
        }
    }

}
