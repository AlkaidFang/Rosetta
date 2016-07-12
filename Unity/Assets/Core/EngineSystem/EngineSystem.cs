using System;
using System.Collections.Generic;
using System.Linq;

namespace Alkaid
{
    public class EngineSystem : Singleton<EngineSystem>, Lifecycle
    {
        private int mFPS = 30;

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

        public void SetFPS(int fps)
        {
            if (fps > 0)
            {
                mFPS = fps;
            }
        }
        public int GetFPS()
        {
            return mFPS;
        }
    }
}
