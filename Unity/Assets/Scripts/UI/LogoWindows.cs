using UnityEngine;
using System.Collections;
using Alkaid;

public class LogoWindows : MonoBehaviour {

    public int showTimes = 3;

	// Use this for initialization
	void Start ()
    {
        LogoCountDown();
    }

    private void LogoCountDown()
    {
        --showTimes;
        if (showTimes < 0)
        {
            WindowManager.Instance.ShowWindow("CGWindow");
            WindowManager.Instance.HideWindow("LogoWindow");
        }
        else
        {
            Invoke("LogoCountDown", 1);
        }
    }
}
