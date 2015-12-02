using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class Singleton<Type> where Type : new()
    {
        private volatile static object instance = null;
        private static Object m_lock = new Object();

        private static Type GetInstance()
        {
            if (null == instance)
            {
                lock (m_lock)
                {
                    if (null == instance)
                    {
                        instance = new Type();
                    }
                }
            }

            return (Type)instance;
        }

        public static Type Instance
        {
            get { return GetInstance(); }
        }
    }
}
