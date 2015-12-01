using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    class SafeList<T>
    {
        private List<T> handlerList;

        public int Count
        {
            get { return handlerList.Count; }
        }

        public SafeList()
        {
            handlerList = new List<T>();
        }

        public void Add(T value)
        {
            lock (handlerList)
            {
                handlerList.Add(value);
            }
        }

        public void Remove(T value)
        {
            lock (handlerList)
            {
                handlerList.Remove(value);
            }
        }

        public List<T> ToList()
        {
            lock (handlerList)
            {
                return handlerList.ToList();
            }
        }

        public T Find(Predicate<T> match)
        {
            lock (handlerList)
            {
                return handlerList.Find(match);
            }
        }

        public void Clear()
        {
            lock (handlerList)
            {
                handlerList.Clear();
            }
        }

    }
}
