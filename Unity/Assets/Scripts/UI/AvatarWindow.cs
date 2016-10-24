using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Alkaid;

public class AvatarWindow : BaseWindow {
	
	public Toggle[] avatarToggles;

	private int selectIndex;

	public override bool Init()
	{
		selectIndex = 0;

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

	public void OnToggleValueChanged(bool status)
	{
		for (int i = 0; i < avatarToggles.Length; ++i) {
			Toggle t = avatarToggles [i];
			if (t.isOn) {
				selectIndex = i;
			}
		}
	}

	public void OnEnterClick()
	{
		// 选中了第几个avatar
		string avatarGuid = "";

	}
}
