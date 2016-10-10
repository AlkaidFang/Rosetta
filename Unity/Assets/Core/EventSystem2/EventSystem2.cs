using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
	public class EventSystem2 : Singleton<EventSystem2>, Lifecycle
	{
		private const int MAX_PROCESS_PER_TICK = 10;
		private Dictionary<int, List<EventHandler2>> mEventHandlerMap;
		private SimplePool<Event2> mEventPool;
		private List<Event2> mFiredEventList;

		public EventSystem2()
		{
			mEventHandlerMap = new Dictionary<int, List<EventHandler2>> ();
			mEventPool = new SimplePool<Event2> ();
			mFiredEventList = new List<Event2> ();
		}

		public bool Init()
		{
			LoggerSystem.Instance.Info("EventSystemSimple    init  begin");

			mEventHandlerMap.Clear ();

			LoggerSystem.Instance.Info("EventSystemSimple    init  end");
			return true;
		}

		public void Tick(float interval)
		{
			for (int i = 0; i < MAX_PROCESS_PER_TICK; ++i)
			{
				if (i >= mFiredEventList.Count)
					break;

				TrigEvent (mFiredEventList [i]);
				mFiredEventList.RemoveAt (i);
			}
		}

		public void Destroy()
		{
			LoggerSystem.Instance.Info("EventSystemSimple    destroy  begin");

			mEventHandlerMap.Clear ();
			for (int i = 0; i < mFiredEventList.Count; ++i)
			{
				mEventPool.Recycle (mFiredEventList [i]);
			}
			mFiredEventList.Clear ();

			LoggerSystem.Instance.Info("EventSystemSimple    destroy  end");
		}

		public void RegisterEvent (EventId id, object hoster, object data, Callback<int, object, object[]> handler)
		{
			RegisterEvent ((int)id, hoster, data, handler);
		}

		public void RegisterEvent (int key, object hoster, object data, Callback<int, object, object[]> handler)
		{
			EventHandler2 e = new EventHandler2();
			e.Init(key, hoster, data, handler);

			List<EventHandler2> total = null;
			if (this.mEventHandlerMap.ContainsKey(key))
			{
				total = this.mEventHandlerMap[key];
			}
			else
			{
				total = new List<EventHandler2>();
				this.mEventHandlerMap.Add(key, total);
			}

			total.Add(e);
		}

		public void UnRegisterEvent (EventId id, object hoster)
		{
			UnRegisterEvent ((int)id, hoster);
		}

		public void UnRegisterEvent (int key, object hoster)
		{
			List<EventHandler2> total = null;
			if (this.mEventHandlerMap.TryGetValue(key, out total))
			{
				List<EventHandler2> temp = new List<EventHandler2>();
				temp.AddRange(total);
				for (int i = 0; i < temp.Count; ++i)
				{
					EventHandler2 handler = temp [i];
					if (handler != null && handler.IsEvent(key, hoster))
					{
						total.Remove(handler);
					}
				}
			}
		}

		public void FireEvent(EventId id, params object[] args)
		{
			FireEvent ((int)id, args);
		}

		public void FireEvent(int key, params object[] args)
		{
			// 先判断目前的事件队列中是否有这个事件，如果有的话，则刷新为最新。因为在同一帧中两次事件的没有作用
			Event2 e = null;
			for (int i = 0; i < mFiredEventList.Count; ++i)
			{
				if (mFiredEventList [i].GetKey () == key) {
					e = mFiredEventList [i];
					break;
				}
			}

			if (e == null) {
				e = mEventPool.Alloc ();
				e.Set (key, args);
				mFiredEventList.Add (e);
			} else {
				e.Set(key, args);
			}
		}

		private void TrigEvent(Event2 e)
		{
			List<EventHandler2> total = null;
			if (this.mEventHandlerMap.TryGetValue(e.GetKey(), out total))
			{
				EventHandler2 eh = null;
				for(int i = 0; i < total.Count; ++i)
				{
					eh = total[i];
					if (eh != null)
					{
						eh.Fire(e);
						mEventPool.Recycle (e);
					}
				}
			}
			else
			{
				LoggerSystem.Instance.Error("No register of this event. key:" + e.GetKey());
			}
		}

	}
}
