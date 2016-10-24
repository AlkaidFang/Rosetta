
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Alkaid;

public enum NetCtr
{
    Lobby = 1,
    Room,
}

public class Rosetta : Singleton<Rosetta>, Lifecycle
{

    public bool Init()
    {
        // 文件位置
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

        // console输出
        LoggerSystem.Instance.SetConsoleLogger(new Alkaid.Logger(UnityEngine.Debug.Log));

        // 网络
        PacketFormat pf = new PacketFormat();
        PacketHandlerManager pm = new PacketHandlerManager();
        NetSystem.Instance.RegisterConnector((int)NetCtr.Lobby, ConnectionType.TCP, pf, pm, ConnectedCallback, null, DisConnectedCallback, null);
//		NetSystem.Instance.RegisterConnector((int)NetCtr.Room, ConnectionType.TCP, pf, pm, conne)

        return Framework.Instance.Init();
    }

    public void Tick(float interval)
    {
        Framework.Instance.Tick(interval);
    }

    public void Destroy()
    {
        Framework.Instance.Destroy();
    }

    private void ConnectedCallback(bool status)
    {
        //EventSystem.Instance.FireEvent("network", "testwindow", status);
		EventSystem2.Instance.FireEvent ((int)EventId.Network, status);
    }

    private void DisConnectedCallback()
    {
		//EventSystem.Instance.FireEvent("network", "testwindow", false);
		EventSystem2.Instance.FireEvent ((int)EventId.Network, false);
    }

    public IEnumerator ConnectLobby(string ip, int port)
    {
        NetSystem.Instance.Connect((int)NetCtr.Lobby, ip, port);
        while(!NetSystem.Instance.GetConnector((int)NetCtr.Lobby).IsConnected())
            yield return 1;
    }

    public void DisconnectLobby()
    {
        NetSystem.Instance.GetConnector((int)NetCtr.Lobby).DisConnect();
    }

    public void Ping()
    {
        NetPacket pa = new NetPacket(PacketType.CS_Ping);
        XMessage.CS_Ping proto = new XMessage.CS_Ping();
        proto.Timestamp = DateTime.Now.ToBinary();
        pa.Proto = proto;
        NetSystem.Instance.Send((int)NetCtr.Lobby, pa);
    }
}



