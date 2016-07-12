using UnityEngine;
using System;
using Alkaid;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Rosetta.Instance.Init();
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
