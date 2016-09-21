using UnityEngine;
using System;
using Alkaid;

public class Main : MonoBehaviour
{
    void Awake()
    {

        // 初始化
        if (Rosetta.Instance.Init())
        {
            LoggerSystem.Instance.Info("启动成功！");
        }
        else
        {
            LoggerSystem.Instance.Error("启动失败！");
        }
    }

    void Start()
    {
        WindowManager.Instance.ShowWindow("LogoWindow");
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
