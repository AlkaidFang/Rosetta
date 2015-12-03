using System;
using System.Collections.Generic;
using System.Linq;

namespace Alkaid
{
    public class EngineSystem : Singleton<EngineSystem>, Lifecycle
    {

        public bool Init()
        {
            LoggerSystem.Instance.Info("EngineSystem    init  begin");


            LoggerSystem.Instance.Info("EngineSystem    init  end");
            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {
            LoggerSystem.Instance.Info("EngineSystem    destroy  begin");


            LoggerSystem.Instance.Info("EngineSystem    destroy  end");

        }

    }
}
