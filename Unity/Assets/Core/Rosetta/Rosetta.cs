/**
 * Rosetta视为RenderThread
 * Framework视为LogicThread
 * */
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Alkaid
{
    public enum NetCtr
    {
        Lobby = 1,
        Room,
    }

    public class Rosetta : Singleton<Rosetta>, Lifecycle
    {
        public void RenderTick(float interval)
        {
        }

        public bool Init()
        {
            // DataProviderSystem
            DataProviderSystem.Instance.RegisterDataProvider(DictionaryDataProvider.Instance);
            DataProviderSystem.Instance.RegisterDataProvider(UIWindowDataProvider.Instance);

            // LocalStorageSystem
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalAccountStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalServerStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalUnVerifyIAPStorage.Instance);

            LoggerSystem.Instance.SetConsoleLogger(new Alkaid.Logger(UnityEngine.Debug.Log));

            if (Application.isEditor)
            {
                FrameworkSetup.Instance.SetWritableRootDir(Application.temporaryCachePath);
                FrameworkSetup.Instance.SetStreamAssetsRootDir(Application.streamingAssetsPath);
            }
            else
            {
                FrameworkSetup.Instance.SetWritableRootDir(Application.temporaryCachePath);
                FrameworkSetup.Instance.SetStreamAssetsRootDir(Application.streamingAssetsPath);
            }

            // NetSystem
            PacketFormat pf = new PacketFormat();
            PacketHandlerManager pm = new PacketHandlerManager();
            pm.RegisterHandler(typeof(XMessage.SC_HelloWorldResult), new SCHelloWorldResultHandler());
            NetSystem.Instance.RegisterConnector((int)NetCtr.Lobby, ConnectionType.TCP, pf, pm, null, null, null, null);
            

            // Init
            bool ret = Framework.Instance.Init();
            if (!ret)
            {
                LoggerSystem.Instance.Fatal("Framework init failed.");
                
                return false;
            }

            return true;
        }

        public void Tick(float interval)
        {
            Framework.Instance.Tick(interval);
        }

        public void Destroy()
        {
            Framework.Instance.Destroy();
        }

    }
}



