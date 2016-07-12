/**
 * Rosetta视为RenderThread
 * Framework视为LogicThread
 * */
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Alkaid
{
    public class Rosetta : Singleton<Rosetta>, Lifecycle
    {
        public enum NetCtr
        {
            Lobby = 1,
            Room,
        }

        public bool Init()
        {
            if (Application.isEditor)
            {
                Framework.Instance.SetWritableRootDir(Application.temporaryCachePath);
                Framework.Instance.SetStreamAssetsRootDir(Application.streamingAssetsPath);
            }
            else
            {
                Framework.Instance.SetWritableRootDir(Application.temporaryCachePath);
                Framework.Instance.SetStreamAssetsRootDir(Application.streamingAssetsPath);
            }

            // DataProviderSystem
            DataProviderSystem.Instance.RegisterDataProvider(DictionaryDataProvider.Instance);
            DataProviderSystem.Instance.RegisterDataProvider(UIWindowDataProvider.Instance);

            // LocalStorageSystem
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalAccountStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalServerStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalUnVerifyIAPStorage.Instance);

            // LoggerSystem
            LoggerSystem.Instance.SetConsoleLogger(new Alkaid.Logger(UnityEngine.Debug.Log));
            
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



