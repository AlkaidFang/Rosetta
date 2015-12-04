using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class EventManager : Lifecycle
    {
        private Dictionary<string, List<Event>> mEventHandlerMap;

        private Dictionary<string, object[]> mFiredEventList;

        public EventManager()
        {
            mEventHandlerMap = new Dictionary<string, List<Event>>();
            mFiredEventList = new Dictionary<string, object[]>();
        }

        public bool Init()
        {
            mEventHandlerMap.Clear();

            return true;
        }

        public void Tick(float interval)
        {
            foreach (var key in mFiredEventList.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    DirectFire(key, mFiredEventList[key]);
                }
            }

            mFiredEventList.Clear();
        }

        public void Destroy()
        {

            mEventHandlerMap.Clear();
        }

        private void DirectFire(string key, params object[] parameters)
        {
            List<Event> total = null;
            if (this.mEventHandlerMap.TryGetValue(key, out total))
            {
                Event e = null;
                for(int i = 0; i < total.Count; ++i)
                {
                    e = total[i];
                    if (e != null)
                    {
                        e.Fire(key, parameters);
                    }
                }
            }
        }

        public void RegisterEvent(string key, object hoster, Delegate handler)
        {
            Event e = new Event();
            e.Init(key, hoster, handler);

            List<Event> total = null;
            if (this.mEventHandlerMap.ContainsKey(key))
            {
                total = this.mEventHandlerMap[key];
            }
            else
            {
                total = new List<Event>();
                this.mEventHandlerMap.Add(key, total);
            }

            total.Add(e);
        }

        public void UnRegisterEvent(string key, object hoster)
        {
            if (this.mEventHandlerMap.ContainsKey(key))
            {
                List<Event> total = this.mEventHandlerMap[key];
                List<Event> temp = new List<Event>();
                temp.AddRange(total);
                foreach(var i in temp)
                {
                    if (i != null && i.IsEvent(key, hoster))
                    {
                        total.Remove(i);
                    }
                }
            }
        }

    }
}
