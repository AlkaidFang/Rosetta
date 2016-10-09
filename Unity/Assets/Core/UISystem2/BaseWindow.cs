using UnityEngine;
using System;
using System.Collections.Generic;
using Alkaid;

public abstract class BaseWindow : MonoBehaviour
{
	/// <summary>
	/// 当前窗口已经注册的事件，用于release时取消注册
	/// </summary>
	private List<EventId> mRegisteredEvents;

	/// <summary>
	/// 窗口的配置数据
	/// </summary>
	private UIWindowData mConfigData;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseWindow"/> class.
	/// </summary>
	public BaseWindow ()
	{
		mRegisteredEvents = new List<EventId> ();
		mConfigData = null;
	}

	/// <summary>
	/// 注册事件
	/// </summary>
	/// <param name="eventId">Event identifier.</param>
	protected void RegisterEvent (EventId eventId)
	{
		// 注册到eventsystem中
		EventSystem2.Instance.RegisterEvent(eventId, this, mConfigData.mName, UISystem2.Instance.OnEventHandler);

		mRegisteredEvents.Add (eventId);
	}

	/// <summary>
	/// 取消注册事件
	/// </summary>
	/// <param name="eventId">Event identifier.</param>
	private void UnRegisterEvent(EventId eventId)
	{
		// 从eventsystem中删除注册事件
		EventSystem2.Instance.UnRegisterEvent(eventId, this);

		mRegisteredEvents.Remove (eventId);
	}

	/// <summary>
	/// 取消所有注册的事件
	/// </summary>
	private void UnRegisterAllEvent()
	{
		for (int i = mRegisteredEvents.Count - 1; i >= 0; --i)
		{
			UnRegisterEvent (mRegisteredEvents [i]);
		}
	}

	/// <summary>
	/// 设置窗口配置数据
	/// </summary>
	/// <param name="data">Data.</param>
	public void SetConfigData(UIWindowData data)
	{
		mConfigData = data;
	}

	/// <summary>
	/// 获得窗口配置数据
	/// </summary>
	/// <returns>The config data.</returns>
	public UIWindowData GetConfigData()
	{
		return mConfigData;
	}

	/// <summary>
	/// 获得窗口的名字
	/// </summary>
	/// <returns>The name.</returns>
	public string GetName()
	{
		return mConfigData == null ? string.Empty : mConfigData.mName;
	}

	/// <summary>
	/// Init this instance.窗口GameObject加载后调用，子类在其中注册事件
	/// </summary>
	public virtual bool Init ()
	{
		return true;
	}

	/// <summary>
	/// Release this instance.窗口GameObject销毁前调用，子类中做一些销毁设置
	/// </summary>
	public virtual void Release()
	{
		UnRegisterAllEvent ();
	}

	/// <summary>
	/// 每次showwindow均会调用
	/// </summary>
	public abstract void OnShow ();

	/// <summary>
	/// 每次hidewindow均调用
	/// </summary>
	public abstract void OnHide ();

	/// <summary>
	/// 窗口事件响应处理方法
	/// </summary>
	/// <param name="eventId">Event identifier.</param>
	/// <param name="args">Arguments.</param>
	public abstract void OnUIEventHandler (int eventId, params object[] args);

}
