using UnityEngine;
using System;
using System.Reflection;
using Alkaid;
using Rosetta;

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

        Rosetta.Rosetta r = Rosetta.Rosetta.Instance;
        Debug.LogWarning("Rosetta  100   " + r.GetRandomNum100());
        Debug.LogWarning("Rosetta  1000   " + r.GetRandomNum1000());

        Rosetta.Rosetta.Instance.Init(SetUpWithUnity);
	}

    private void SetUpWithUnity()
    {
        LoggerSystem.Instance.SetConsoleLogger(new Logger(UnityEngine.Debug.Log));
        
        if (Application.isEditor)
        {
            LoggerSystem.Instance.SetFileLogPath(Application.temporaryCachePath);
            FrameworkSetup.Instance.SetStreamAssetsRootDir(Application.streamingAssetsPath);
        }
        else
        {
            LoggerSystem.Instance.SetFileLogPath(Application.temporaryCachePath);
            FrameworkSetup.Instance.SetStreamAssetsRootDir(Application.persistentDataPath);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Rosetta.Rosetta.Instance.Tick(Time.deltaTime);
	}

    void OnDestroy()
    {
        Rosetta.Rosetta.Instance.Destroy();
    }

    private int count = 0;
    public void OnClick()
    {
        if (count == 0)
        {
            NetSystem.Instance.Connect((int)RosettaSetup.NetCtr.Lobby, "10.12.25.205", 10086);
        }
        if (count > 0)
        {
            NetPacket pa = new NetPacket(PacketType.CS_HelloWorld);
            XMessage.HelloWorld proto = new XMessage.HelloWorld();
            proto._int = count + 70000;
            pa.mProto = proto;
            pa.EncodeProto();
            NetSystem.Instance.Send((int)RosettaSetup.NetCtr.Lobby, pa);
        }

        Alkaid.MyRandom mr = new Alkaid.MyRandom();
        Debug.LogWarning("Framework 1000   " + mr.GetRandomNum1000());

        GameObject go = mr.CreateOne();
        //go.transform.parent = this.transform;
        go.name = "createone:" + count++;
        go.transform.localPosition = Vector3.one * count;

        

    }
}
