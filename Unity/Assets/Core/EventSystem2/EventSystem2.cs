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
		private List<Event2> mFiredEventList;

		public EventSystem2()
		{
			mEventHandlerMap = new Dictionary<int, List<EventHandler2>> ();
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

				DirectFire (mFiredEventList [i]);
				mFiredEventList.RemoveAt (i);
			}
		}

		public void Destroy()
		{
			LoggerSystem.Instance.Info("EventSystemSimple    destroy  begin");

			mEventHandlerMap.Clear ();

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
			Event2 e = new Event2(key, args);
			DelayFire (e, 1);
		}

		private void DirectFire(Event2 e)
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
					}
				}
			}
			else
			{
				LoggerSystem.Instance.Error("No register of this event. key:" + e.GetKey());
			}
		}

		private void DelayFire(Event2 e, int delay)
		{
			mFiredEventList.Add(e);
		}
	}
}
