using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class EventSystem : Singleton<EventSystem>, Lifecycle
    {
        public bool Init()
        {

            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {

        }

    }
}
