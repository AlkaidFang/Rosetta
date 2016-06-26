using UnityEngine;
using System.Collections;
using Alkaid;

public class LogoWindows : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    float showTimes = 3.0f;

	// Update is called once per frame
	void Update () {
        if (showTimes >= 0)
        {
            showTimes -= Time.deltaTime;
            if (showTimes < 0)
            {
                WindowManager.Instance.ShowWindow("TestWindow");
            }
        }

	}
}
