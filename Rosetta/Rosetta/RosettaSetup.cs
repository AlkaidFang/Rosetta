using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class RosettaSetup : InstanceTemplate<RosettaSetup>, Lifecycle
    {
        public RosettaSetup()
        {

        }

        public bool Init()
        {
            // 对FrameworkConfig中的内容进行设置
            FrameworkConfig.Instance.SetFPS(30);
            FrameworkConfig.Instance.SetClearUICache(true);
            //FrameworkConfig.Instance.SetLoggerSystem(UnityEn);
            FrameworkConfig.Instance.SetLoggerSystemLevel((int)Alkaid.LoggerSystem.LogLevel.Info);

            return true;
        }

        public void Tick()
        {

        }

        public void Destroy()
        {

        }

    }
}
