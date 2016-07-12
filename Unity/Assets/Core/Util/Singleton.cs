using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Singleton<T> where T : new()
    {
        private volatile static object instance = null;
        private static Object m_lock = new Object();

        private static T GetInstance()
        {
            if (null == instance)
            {
                lock (m_lock)
                {
                    if (null == instance)
                    {
                        instance = new T();
                    }
                }
            }

            return (T)instance;
        }

        public static T Instance
        {
            get { return GetInstance(); }
        }
    }
}
