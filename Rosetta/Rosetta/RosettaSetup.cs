using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class RosettaSetup : Singleton<RosettaSetup>
    {
        public enum NetCtr
        {
            Lobby = 1,
            Room,
        }

        public void SetupWithUnity()
        {

        }

        public void SetupWithProject()
        {
            // 对FrameworkSetup中的内容进行设置
            //FrameworkSetup.Instance.SetFPS(30);
            //FrameworkSetup.Instance.SetClearUICache(true);

            // LoggerSystem
            //LoggerSystem.Instance.SetLogLevel((int)LoggerSystem.LogLevel.LOG_LEVEL_DEBUG);
            //LoggerSystem.Instance.SaveFileLog(true);

            // DataProviderSystem
            DataProviderSystem.Instance.RegisterDataProvider(DictionaryDataProvider.Instance);
            DataProviderSystem.Instance.RegisterDataProvider(UIWindowDataProvider.Instance);

            // LocalStorageSystem
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalAccountStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalServerStorage.Instance);
            LocalStorageSystem.Instance.RegisterLocalStorage(LocalUnVerifyIAPStorage.Instance);

            // NetSystem
            PacketFormat pf = new PacketFormat();
            PacketHandlerManager pm = new PacketHandlerManager();
            NetSystem.Instance.RegisterConnector((int)NetCtr.Lobby, NetSystem.NetType.TCP, pf, null, (ptype, data) => { pm.DispatchHandler(ptype, data); }, null, null);
            NetSystem.Instance.RegisterConnector((int)NetCtr.Room, NetSystem.NetType.TCP, pf, null, (ptype, data) => { pm.DispatchHandler(ptype, data); }, null, null);
            pm.RegisterHandler(typeof(XMessage.HelloWorldResult), new SCHelloWorldResultHandler());
        }

    }
}
