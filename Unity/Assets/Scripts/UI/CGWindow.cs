using System;
using System.Collections;
using UnityEngine;
using Alkaid;

public class CGWindow : MonoBehaviour
{
    void Start()
    {
        this.StartCoroutine(PlayCG());
    }

    private IEnumerator PlayCG()
    {
#if !UNITY_EDITOR
        Handheld.PlayFullScreenMovie("cg/cg.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForSeconds(0.1f);
#else
        yield return 0;
#endif
        WindowManager.Instance.ShowWindow("TestWindow");
        WindowManager.Instance.HideWindow("CGWindow");
        Debug.LogWarning("视屏播放结束");
    }

}
