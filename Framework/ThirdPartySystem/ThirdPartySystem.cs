using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class ThirdPartySystem : Singleton<ThirdPartySystem>, Lifecycle
    {
        /**
         * We will use this system to do sdk method, every call will be collected to this obj.
         * 
         * */

        public ThirdPartySystem()
        {

        }

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
