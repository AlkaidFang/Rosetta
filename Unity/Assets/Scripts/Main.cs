using UnityEngine;
using System;
using Alkaid;

public class Main : MonoBehaviour
{
    void Awake()
    {
        if (Rosetta.Instance.Init())
        {
            LoggerSystem.Instance.Info("启动成功！");
        }
        else
        {
            LoggerSystem.Instance.Error("启动失败！");
        }
    }

    void Update()
    {
        Rosetta.Instance.Tick(Time.deltaTime);
    }

    void OnDestroy()
    {
        Rosetta.Instance.Destroy();
    }
}
