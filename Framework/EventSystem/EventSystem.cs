using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class EventSystem : Singleton<EventSystem>, Lifecycle
    {
        private Dictionary<int, EventManager> mThreadEventManagerMap;
        private string _Format4Key = "{0}@{1}";

        public EventSystem()
        {
            mThreadEventManagerMap = new Dictionary<int,EventManager>();
        }

        public bool Init()
        {
            LoggerSystem.Instance.Info("EventSystem    init  begin");

            mThreadEventManagerMap.Clear();


            LoggerSystem.Instance.Info("EventSystem    init  end");
            return true;
        }

        public void Tick(float interval)
        {
            int uid = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (mThreadEventManagerMap.ContainsKey(uid))
            {
                mThreadEventManagerMap[uid].Tick(interval);
            }
        }

        public void Destroy()
        {
            LoggerSystem.Instance.Info("EventSystem    destroy  begin");
            foreach (var em in mThreadEventManagerMap.Values)
            {
                em.Destroy();
            }

            mThreadEventManagerMap.Clear();
            LoggerSystem.Instance.Info("EventSystem    destroy  end");
        }

        private string FormatKey(string arg0, string arg1)
        {
			return string.Format(_Format4Key, arg0, arg1);
        }

		private void RegisterEvent(string name, string group, Delegate handler, object hoster)
        {
            int uid = System.Threading.Thread.CurrentThread.ManagedThreadId;
            EventManager em = null;
            if (!mThreadEventManagerMap.TryGetValue(uid, out em))
            {
                em = new EventManager();
                em.Init();
                mThreadEventManagerMap.Add(uid, em);
            }
            
            if (em != null)
            {
                em.RegisterEvent(FormatKey(name, group), hoster, handler);
            }
        }

		public void RegisterEvent(string name, string group, Callback handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}

		public void RegisterEvent<T0>(string name, string group, Callback<T0> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1>(string name, string group, Callback<T0, T1> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1, T2>(string name, string group, Callback<T0, T1, T2> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1, T2, T3>(string name, string group, Callback<T0, T1, T2, T3> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1, T2, T3, T4>(string name, string group, Callback<T0, T1, T2, T3, T4> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1, T2, T3, T4, T5>(string name, string group, Callback<T0, T1, T2, T3, T4, T5> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}
		public void RegisterEvent<T0, T1, T2, T3, T4, T5, T6>(string name, string group, Callback<T0, T1, T2, T3, T4, T5, T6> handler, object hoster)
		{
			RegisterEvent (name, group, handler, hoster);
		}

        public void UnRegisterEvent(string name, string group, object hoster)
        {
            int uid = System.Threading.Thread.CurrentThread.ManagedThreadId;
            EventManager em = null;
            if (mThreadEventManagerMap.TryGetValue(uid, out em) && em != null)
            {
                em.UnRegisterEvent(FormatKey(name, group), hoster);
            }
        }

        /**
         * 此处给自己的线程的管理器直接发送消息，对其他的管理器发送延时
         * 由于thread的hashcode只能给自己线程的事件管理器发送事件
         * 所以没办法才这样做的啊，要理解设计时的痛苦，这个地方没见过别人怎么做的
         * */
        public void FireEvent(string name, string group, params object[] args)
        {
            int uid = System.Threading.Thread.CurrentThread.ManagedThreadId;

            EventManager em = null;
            foreach (var key in mThreadEventManagerMap.Keys)
            {
                em = mThreadEventManagerMap[key];
                if (em == null) continue;

                if (key == uid)
                {
                    em.FireEvent(FormatKey(name, group), args);
                }
                else
                {
                    em.FireEvent2(FormatKey(name, group), args);
                }
            }
        }

        public void FireEvent2(string name, string group, params object[] args)
        {
            int uid = System.Threading.Thread.CurrentThread.ManagedThreadId;

            EventManager em = null;
            foreach (var key in mThreadEventManagerMap.Keys)
            {
                em = mThreadEventManagerMap[key];
                if (em == null) continue;

                em.FireEvent2(FormatKey(name, group), args);
            }
        }


    }
}
