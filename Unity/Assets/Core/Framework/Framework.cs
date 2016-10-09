using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alkaid
{
    public class Framework : Singleton<Framework>, Lifecycle
    {
        private string mVersion = string.Empty;
        private string mStreamAssetsRootDir = string.Empty;
        private string mWritableRootDir = string.Empty;

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
				if (!EventSystem2.Instance.Init()) break;
				if (!LocalStorageSystem.Instance.Init()) break;
				if (!UISystem.Instance.Init()) break;
				if (!UISystem2.Instance.Init()) break;
                if (!NetSystem.Instance.Init()) break;
                if (!ThirdPartySystem.Instance.Init()) break;


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
			EventSystem2.Instance.Tick(interval);
            LocalStorageSystem.Instance.Tick(interval);
			UISystem.Instance.Tick (interval);
			UISystem2.Instance.Tick (interval);
            NetSystem.Instance.Tick(interval);
            ThirdPartySystem.Instance.Tick(interval);
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
			UISystem2.Instance.Destroy();
            NetSystem.Instance.Destroy();
            ThirdPartySystem.Instance.Destroy();

            LoggerSystem.Instance.Info("Framework destroy end.");

            LoggerSystem.Instance.Info("========================Good bye!========================");
            
            LoggerSystem.Instance.Destroy();
        }


        public void SetVersion(string version)
        {
            mVersion = version;
            LocalStorageSystem.Instance.SetAppVersion(mVersion);
        }

        public string GetVersion()
        {
            return mVersion;
        }

        public void SetStreamAssetsRootDir(string path)
        {
            mStreamAssetsRootDir = path;
        }

        public string GetStreamAssetsRootDir()
        {
            return mStreamAssetsRootDir;
        }

        public void SetWritableRootDir(string path)
        {
            mWritableRootDir = path;
        }

        public string GetWritableRootDir()
        {
            return mWritableRootDir;
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
