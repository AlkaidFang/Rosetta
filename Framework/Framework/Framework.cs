using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alkaid
{
    public class Framework : Singleton<Framework>, Lifecycle
    {
        private AsyncThreading mLogicThread;

        public Framework()
        {
            mLogicThread = null;
        }

        /**
         * This function will be call in renderthread
         * */
        public bool Init()
        {
            do
            {
                if (!ConfigSystem.Instance.Init()) break;
                if (!LoggerSystem.Instance.Init()) break;
                LoggerSystem.Instance.Info("========================Hello!========================");
                LoggerSystem.Instance.Info("Framework    init begin.");
                if (!TimeSystem.Instance.Init()) break;
                if (!DataProviderSystem.Instance.Init()) break;
                if (!EngineSystem.Instance.Init()) break;
                if (!EventSystem.Instance.Init()) break;
                if (!LocalStorageSystem.Instance.Init()) break;
                if (!UISystem.Instance.Init()) break;


                LoggerSystem.Instance.Info("Framework init end.");
                return true;
            }
            while(false);

            return false;
        }

        /**
         * This function will be call in logicthread
         * */
        public void Tick(float interval)
        {
            LoggerSystem.Instance.Tick(interval);
            DataProviderSystem.Instance.Tick(interval);
            EngineSystem.Instance.Tick(interval);
            TimeSystem.Instance.Tick(interval);
            EventSystem.Instance.Tick(interval);
            LocalStorageSystem.Instance.Tick(interval);
        }

        /**
         * This function will be call in renderthread
         * */
        public void Destroy()
        {
            LoggerSystem.Instance.Info("Framework destroy begin");

            EngineSystem.Instance.Destroy();
            EventSystem.Instance.Destroy();
            LocalStorageSystem.Instance.Destroy();
            DataProviderSystem.Instance.Destroy();
            TimeSystem.Instance.Destroy();
            UISystem.Instance.Destroy();

            LoggerSystem.Instance.Info("Framework destroy end.");

            LoggerSystem.Instance.Info("========================Good bye!========================");
            
            LoggerSystem.Instance.Destroy();
        }

        private void Logic(AsyncThreading thread)
        {
            LoggerSystem.Instance.Info("Logic Thread start.");

            int fps = FrameworkSetup.Instance.GetFPS();
            int constSleepTime = 1000 / fps;
            LoggerSystem.Instance.Info("-------------------------------------------------");
            LoggerSystem.Instance.Info("Logic Thread run at FPS:" + fps + ",  frame time is:" + constSleepTime + "ms.");
            LoggerSystem.Instance.Info("Everything is ready, Let's play!");
            LoggerSystem.Instance.Info("-------------------------------------------------");

            TimeSpan during = new TimeSpan();
            DateTime tickStart = DateTime.Now;
            while (thread.IsWorking())
            {
                during = (DateTime.Now - tickStart); // 上一帧所消耗的时间
                tickStart = DateTime.Now;

                Tick((float)during.TotalSeconds);

                during = DateTime.Now - tickStart; // 当前tick逻辑所消耗的时间
                if (constSleepTime > during.TotalMilliseconds)
                {
                    // 为了帧率稳定
                    System.Threading.Thread.Sleep(constSleepTime - (int)during.TotalMilliseconds);
                }
            }

            LoggerSystem.Instance.Info("Logic Thread finished.");
        }

        public void Start()
        {
            // setup
            FrameworkSetup.Instance.Apply();

            // Init
            bool ret = Init();
            if (!ret)
            {
                LoggerSystem.Instance.Fatal("Framework init failed.");
                Stop();
                return;
            }

            // Logic Thread start
            mLogicThread = new AsyncThreading(Logic);
            if (mLogicThread != null)
            {
                mLogicThread.Start();
            }
        }

        public void Stop()
        {
            if (mLogicThread != null)
            {
                mLogicThread.SyncStop(Destroy);
            }
            else
            {
                Destroy();
            }
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
