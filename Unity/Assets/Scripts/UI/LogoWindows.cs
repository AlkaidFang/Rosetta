using UnityEngine;
using System.Collections;
using Alkaid;

public class LogoWindows : BaseWindow {

    public int showTimes = 2;

	public override void OnShow ()
	{
		LogoCountDown ();
	}

	public override void OnHide ()
	{
		
	}

	public override void OnUIEventHandler (int eventId, params object[] args)
	{
		
	}

    private void LogoCountDown()
    {
        --showTimes;
        if (showTimes < 0)
        {
			UISystem2.Instance.ShowWindow("CGWindow");
			UISystem2.Instance.HideWindow("LogoWindow");
        }
        else
        {
            Invoke("LogoCountDown", 1);
        }
    }
}
