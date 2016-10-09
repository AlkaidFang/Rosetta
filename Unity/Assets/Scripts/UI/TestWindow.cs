using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Alkaid;

public class TestWindow : BaseWindow {

    public InputField ipLabel;
    public InputField portLabel;
    public Text connectLabel;
    public Text pingLabel;

	public override bool Init ()
	{
		RegisterEvent (EventId.Network);
		RegisterEvent (EventId.Ping);
		return base.Init ();
	}

	public override void OnShow ()
	{
		connectLabel.text = "unknow";
	}

	public override void OnHide ()
	{
		
	}

	public override void OnUIEventHandler (int eventId, params object[] args)
	{
		if (eventId == (int)EventId.Network) {
			OnNetworkEvent ((bool)args [0]);
		} else if (eventId == (int)EventId.Ping) {
			OnPingEvent ((double)args [0]);
		}
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
