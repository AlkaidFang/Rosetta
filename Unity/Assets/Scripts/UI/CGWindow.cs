using System;
using System.Collections;
using UnityEngine;
using Alkaid;

public class CGWindow : BaseWindow
{
	public override void OnShow ()
	{
		this.StartCoroutine(PlayCG());
	}

	public override void OnHide ()
	{
		
	}

	public override void OnUIEventHandler (int eventId, params object[] args)
	{
		
	}

    private IEnumerator PlayCG()
    {
#if !UNITY_EDITOR
        Handheld.PlayFullScreenMovie("cg/cg.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForSeconds(0.1f);
#else
        yield return 0;
#endif
		UISystem2.Instance.ShowWindow("TestWindow");
		UISystem2.Instance.HideWindow("CGWindow");
        Debug.LogWarning("视屏播放结束");
    }

}
