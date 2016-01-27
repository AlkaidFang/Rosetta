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
            //LoggerSystem.Instance.SetLogLevel((int)LoggerSystem.LogLevel.DEBUG);
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
            pm.RegisterHandler(typeof(XMessage.SC_HelloWorldResult), new SCHelloWorldResultHandler());
            NetSystem.Instance.RegisterConnector((int)NetCtr.Lobby, ConnectionType.WEBSOCKET, pf, pm, null, null, null, null);
            /*NetSystem.Instance.Connect((int)NetCtr.Lobby, "ws://localhost:8080/PearlHarbor/Game", 8080);
            NetPacket packet = new NetPacket(PacketType.CS_HelloWorld);
            XMessage.CS_HelloWorld proto = new XMessage.CS_HelloWorld();
            proto._int = 11;
            proto._string = "helloworld";
            proto._long = 999;
            packet.Proto = proto;
            NetSystem.Instance.Send((int)NetCtr.Lobby, packet);*/


            /*NetSystem.Instance.RegisterConnector((int)NetCtr.Lobby, ConnectionType.TCP, pf, pm, null, null, null, null);
            NetSystem.Instance.RegisterConnector((int)NetCtr.Room, ConnectionType.TCP, pf, pm, null, null, null, null);

            // do connect
            NetSystem.Instance.Connect((int)NetCtr.Lobby, "127.0.0.1", 10086);
            // do send
            NetPacket packet = new NetPacket(PacketType.CS_HelloWorld);
            XMessage.CS_HelloWorld proto = new XMessage.CS_HelloWorld();
            proto._int = 11;
            proto._string = "helloworld";
            packet.Proto = proto;
            NetSystem.Instance.Send((int)NetCtr.Lobby, packet);*/
        }

    }
}
