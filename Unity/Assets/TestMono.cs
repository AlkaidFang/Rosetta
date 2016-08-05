using UnityEngine;
using System;
using System.Reflection;
using Alkaid;

public class TestMono : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Type type = Type.GetType("Mono.Runtime");
		if (type != null)
		{
			MethodInfo info = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
			
			if (info != null)
				Debug.Log(info.Invoke(null, null));
		}

	}	

    private int count = 0;
    public void OnClick()
    {
        if (count == 0)
        {
            //NetSystem.Instance.Connect((int)RosettaSetup.NetCtr.Lobby, "ws://127.0.0.1:8080/PearlHarbor/Game", 8080);
			NetSystem.Instance.Connect((int)Rosetta.NetCtr.Lobby, "120.131.1.171", 8192);
        }
        if (count > 0)
        {
            NetPacket pa = new NetPacket(PacketType.CS_HelloWorld);
            XMessage.CS_HelloWorld proto = new XMessage.CS_HelloWorld();
            proto.Int = count + 70000;
            proto.Float = 0.998f;
            proto.String = "helloworld";
            proto.Long = 22222 - count;
            pa.Proto = proto;
            NetSystem.Instance.Send((int)Rosetta.NetCtr.Lobby, pa);
        }

        Alkaid.MyRandom mr = new Alkaid.MyRandom();
        Debug.LogWarning("Framework 1000   " + mr.GetRandomNum1000());

        GameObject go = mr.CreateOne();
        //go.transform.parent = this.transform;
        go.name = "createone:" + count++;
        go.transform.localPosition = Vector3.one * count;


        WindowManager.Instance.ShowWindow("LogoWindow");
    }
}
