using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Alkaid;

public class TestWindow : MonoBehaviour {

    public InputField ipLabel;
    public InputField portLabel;
    public Text connectLabel;
    public Text pingLabel;

	// Use this for initialization
	void Start () {
        EventSystem.Instance.RegisterEvent<int>("click", "testwindow", eventhandler, this);

        EventSystem.Instance.RegisterEvent<bool>("network", "testwindow", OnNetworkEvent, this);

        EventSystem.Instance.RegisterEvent<double>("ping", "testwindow", OnPingEvent, this);

        connectLabel.text = "unknow";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Destroy()
    {
        EventSystem.Instance.UnRegisterEvent("click", "testwindow", this);

        EventSystem.Instance.UnRegisterEvent("network", "testwindow", this);

        EventSystem.Instance.UnRegisterEvent("ping", "testwindow", this);
    }

    int i = 0;
    public void OnLogoClick()
    {
        LoggerSystem.Instance.Info("You have click this logo!!!");
        EventSystem.Instance.FireEvent("click", "testwindow", i++);
    }

    private void eventhandler(int times)
    {
        LoggerSystem.Instance.Info("recieved event----- times:" + times);
    }

    private void OnNetworkEvent(bool status)
    {
        if (status)
        {
            connectLabel.text = "Connected";
        }
        else
        {
            connectLabel.text = "DisConnected";
        }
    }

    private void OnPingEvent(double ms)
    {
        pingLabel.text = string.Format("{0}ms", (int)ms);
    }

    public void OnConnectClick()
    {
        string ip = ipLabel.text;
        int port = int.Parse(portLabel.text);
        this.StartCoroutine(Rosetta.Instance.ConnectLobby(ip, port));

        Ping();
    }

    public void OnDisconnectClick()
    {
        Rosetta.Instance.DisconnectLobby();
        
    }

    private void Ping()
    {
        Rosetta.Instance.Ping();

        Invoke("Ping", 1);
    }

}
