using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Alkaid;

public class SelectServerWindow : BaseWindow {

	public InputField ipInput;
	public InputField portInput;

	public override bool Init()
	{

		return base.Init ();
	}

	public override void OnShow()
	{
		
	}

	public override void OnHide()
	{
		
	}

	public override void OnUIEventHandler(int eventId, params object[] args)
	{
		
	}

	public void OnLoginClick()
	{
		string ip = ipInput.text;
		int port = int.Parse (portInput.text);

		this.StartCoroutine (Rosetta.Instance.ConnectLobby (ip, port));

		InvokeRepeating ("UpdateConnection", 0.5f, 0.5f);
	}

	public void UpdateConnection()
	{
		INetConnector lobbyConnection = NetSystem.Instance.GetConnector ((int)NetCtr.Lobby);
		ConnectionStatus status = lobbyConnection.GetConnectStatus ();
		if (status == ConnectionStatus.CONNECTING) {
			// 连接中。。。
			Debug.Log ("服务器连接中。。。");
		} else if (status == ConnectionStatus.CONNECTED) {
			// 开始登录

			CancelInvoke ("UpdateConnection");
		} else {
			Debug.Log ("服务器连接失败！");
		}
	}
}
