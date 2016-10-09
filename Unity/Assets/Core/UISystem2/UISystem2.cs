using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Alkaid;

public class UISystem2 : Singleton<UISystem2>, Lifecycle
{
	/// <summary>
	/// 需要管理的窗口
	/// </summary>
	private List<BaseWindow> mManagedWindows;

	/// <summary>
	/// 显示窗口栈
	/// </summary>
	private Stack<BaseWindow> mShowingStack;

	/// <summary>
	/// 窗口根节点
	/// </summary>
	private GameObject mUIRoot;
	private GameObject mUICamera;
	private GameObject mForwardCamera;

	/// <summary>
	/// Initializes a new instance of the <see cref="UISystem"/> class.
	/// </summary>
	public UISystem2 ()
	{
		mManagedWindows = new List<BaseWindow> ();
		mShowingStack = new Stack<BaseWindow> ();

		mUIRoot = null;
		mUICamera = null;
		mForwardCamera = null;
	}

	/// <summary>
	/// 获得所需要的窗口对象，私有借口.
	/// </summary>
	/// <returns>The window.</returns>
	/// <param name="name">Name.</param>
	private BaseWindow GetWindow(string name)
	{
		BaseWindow bw = null;

		for (int i = 0; i < mManagedWindows.Count; ++i)
		{
			if (mManagedWindows [i] != null && mManagedWindows [i].GetName ().Equals(name))
			{
				bw = mManagedWindows [i];
			}
		}

		return bw;
	}

	/// <summary>
	/// 直接加入显示栈中
	/// </summary>
	/// <param name="bw">Bw.</param>
	private void PushShowingStack(BaseWindow bw)
	{
		if (bw == null)
			return;

		mShowingStack.Push (bw);
	}

	/// <summary>
	/// 从显示栈中清除这个界面
	/// tips: 可能界面不是最上层，需要处理
	/// </summary>
	/// <param name="bw">Bw.</param>
	private void PopShowingStack(BaseWindow bw)
	{
		if (bw == null)
			return;

		// 先判断，如果栈顶层的就是当前的，则直接pop
		if (mShowingStack.Peek () == bw)
		{
			mShowingStack.Pop ();
			return;
		}

		Stack<BaseWindow> temp = new Stack<BaseWindow>();
		BaseWindow b = null;
		for (int i = 0; i < mShowingStack.Count; ++i)
		{
			b = mShowingStack.Pop ();
			if (b != bw)
				temp.Push (b);
			else
				break;
		}
		for (int i = 0; i < temp.Count; ++i)
		{
			mShowingStack.Push (temp.Pop());
		}
		temp.Clear ();
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public bool Init()
	{
		mUIRoot = UnityEngine.GameObject.Find("UIRoot");
		if (mUIRoot == null)
		{
			
			return false;
		}

		mUICamera = mUIRoot.transform.FindChild("UICamera").gameObject;
		mForwardCamera = mUIRoot.transform.FindChild("ForwardCamera").gameObject;

		// 设置UISystem的一些数据
		UnityEngine.Object.DontDestroyOnLoad(mUIRoot);

		return true;
	}

	public void Tick(float interval)
	{
		
	}

	public void Destroy()
	{
		LoggerSystem.Instance.Info("UISystem2    destroy  begin");

		HideAllWindow ();

		LoggerSystem.Instance.Info("UISystem2    destroy  end");
	}

	/// <summary>
	/// 判断窗口是否可见
	/// </summary>
	/// <returns><c>true</c> if this window is visible; otherwise, <c>false</c>.</returns>
	/// <param name="window">Window.</param>
	private bool IsWindowVisible(BaseWindow window)
	{
		return window != null && window.gameObject.activeSelf;
	}

	/// <summary>
	/// 判断窗口是否显示
	/// </summary>
	public bool IsWindowVisible(string name)
	{
		BaseWindow bw = GetWindow (name);

		return IsWindowVisible (bw);
	}

	/// <summary>
	/// 显示窗口
	/// tips：检查是否是隐藏
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowWindow(string name)
	{
		// 判断是否是隐藏，如果隐藏，则设置显示，如果已经显示，则不操作
		BaseWindow bw = GetWindow(name);
		if (bw != null)
		{
			if (!IsWindowVisible (bw)) {
				bw.gameObject.SetActive (true);
				bw.OnShow ();
				PushShowingStack (bw);
			}
			return;
		}

		// 获得外部配置数据
		UIWindowData config = UIWindowDataProvider.Instance.GetData(name);
		if (config == null || string.IsNullOrEmpty (config.mPrefabPath))
		{
			Debug.LogErrorFormat ("UISystem_ShowWindow : {0}'s prefab config is null!", name);
			return;
		}

		// 加载prefab，实例化，挂载至root点
		GameObject prefab = Resources.Load(config.mPrefabPath) as GameObject;
		if (null == prefab)
		{
			Debug.LogErrorFormat ("UISystem_ShowWindow : {0}'s prefab cannot Load!", name);
			return;
		}

		prefab = UnityTools.AddChild(UISystem.Instance.GetUICamera(), prefab);

		// 获得BaseWindow对象，调用Init
		bw = prefab.GetComponent<BaseWindow>();
		if (bw == null)
		{
			Debug.LogErrorFormat ("UISystem_ShowWindow : {0} can't get BaseWindow component from prefab!", name);
			return;
		}
		bw.SetConfigData (config);
		if (!bw.Init ())
		{
			Debug.LogErrorFormat ("UISystem_ShowWindow : {0}'s BaseWindow init failed!", name);
			return;
		}

		// 加入管理队列和栈
		bw.OnShow();
		mManagedWindows.Add(bw);
		PushShowingStack (bw);

		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}

	/// <summary>
	/// 隐藏窗口
	/// tips：判断配置数据是否需要直接删除
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideWindow(string name)
	{
		BaseWindow bw = GetWindow (name);
		if (!IsWindowVisible (bw))
		{
			Debug.LogFormat ("UISystem_HideWindow : {0} don't showed or invisible, don't need hide.", name);
			return;
		}

		// 调用
		bw.OnHide ();

		// 先从显示栈中删了 //未预防在destroy后不知道是否还存在对象，移到前面处理 update at 2016-7-11
		PopShowingStack(bw);

		// 判断配置数据是否直接删除
		if (bw.GetConfigData ().mHideWithDestroy)
		{
			// 删除
			bw.Release ();
			mManagedWindows.Remove (bw);
			GameObject.Destroy (bw.gameObject);
		}
		else
		{
			// 隐藏
			bw.gameObject.SetActive (false);
		}

		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}

	/// <summary>
	/// 关闭所有窗口
	/// </summary>
	public void HideAllWindow()
	{
		BaseWindow bw = null;
		for (int i = mManagedWindows.Count - 1; i >= 0; -- i)
		{
			bw = mManagedWindows [i];
			if (bw != null && IsWindowVisible (bw))
			{
				HideWindow (bw.GetName ());
			}
		}
	}

	/// <summary>
	/// 窗口系统中注册事件时的统一回调方法
	/// </summary>
	/// <param name="eventId">Event identifier.</param>
	/// <param name="window">Window.</param>
	/// <param name="args">Arguments.</param>
	public void OnEventHandler(int eventId, object window, params object[] args)
	{
		// 如果窗口存在，就派发事件，不管显示还是隐藏
		BaseWindow bw = GetWindow ((string)window);
		if (bw != null)
		{
			bw.OnUIEventHandler (eventId, args);
		}
	}
}

