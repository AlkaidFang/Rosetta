using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{


    public class EventManager : Lifecycle
    {
        private Dictionary<string, List<EventHandler>> mEventHandlerMap;

        private List<Event> mFiredEventList;
        //private List<Event> 

        public EventManager()
        {
            mEventHandlerMap = new Dictionary<string, List<EventHandler>>();
            mFiredEventList = new List<Event>();
        }

        public bool Init()
        {
            mEventHandlerMap.Clear();

            return true;
        }

        public void Tick(float interval)
        {
            for (int i = mFiredEventList.Count - 1; i >= 0; --i)
            {
                if (mFiredEventList[i].CanFire())
                {
                    DirectFire(mFiredEventList[i]);
                    mFiredEventList.RemoveAt(i);
                }
                else
                {
                    mFiredEventList[i].DoTick();
                }
            }
        }

        public void Destroy()
        {

            mEventHandlerMap.Clear();
        }

        private void DirectFire(Event e)
        {
            List<EventHandler> total = null;
            if (this.mEventHandlerMap.TryGetValue(e.GetKey(), out total))
            {
                EventHandler eh = null;
                for(int i = 0; i < total.Count; ++i)
                {
                    eh = total[i];
                    if (eh != null)
                    {
                        eh.Fire(e.GetKey(), e.GetArgs());
                    }
                }
            }
            else
            {
                LoggerSystem.Instance.Error("Not register this event for EventHandler:" + this.GetHashCode());
            }
        }

        public void RegisterEvent(string key, object hoster, Delegate handler)
        {
            EventHandler e = new EventHandler();
            e.Init(key, hoster, handler);

            List<EventHandler> total = null;
            if (this.mEventHandlerMap.ContainsKey(key))
            {
                total = this.mEventHandlerMap[key];
            }
            else
            {
                total = new List<EventHandler>();
                this.mEventHandlerMap.Add(key, total);
            }

            total.Add(e);
        }

        public void UnRegisterEvent(string key, object hoster)
        {
            if (this.mEventHandlerMap.ContainsKey(key))
            {
                List<EventHandler> total = this.mEventHandlerMap[key];
                List<EventHandler> temp = new List<EventHandler>();
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

        public void FireEvent(string key, params object[] args)
        {
            // 直接执行
            Event e = new Event(key, args);
            DirectFire(e);
            //FireEventDelay(0, key, args);
        }

        public void FireEvent2(string key, params object[] args)
        {
            FireEventDelay(1, key, args);
        }

        private void FireEventDelay(int delay, string key, params object[] args)
        {
            Event e = new Event(key, args, delay);
            mFiredEventList.Add(e);
        }
    }
}
