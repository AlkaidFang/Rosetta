using UnityEngine;
using System.Collections;
using Alkaid;

public class TestWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventSystem.Instance.RegisterEvent<int>("click", "testwindow", eventhandler, this);
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
