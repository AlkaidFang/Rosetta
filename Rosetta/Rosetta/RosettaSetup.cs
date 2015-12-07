using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class RosettaSetup : Singleton<RosettaSetup>
    {
        public void SetupWithUnity()
        {

        }

        public void SetupWithProject()
        {
            // 对FrameworkSetup中的内容进行设置
            FrameworkSetup.Instance.SetFPS(30);
            FrameworkSetup.Instance.SetClearUICache(true);

            // LoggerSystem
            LoggerSystem.Instance.SetLogLevel((int)LoggerSystem.LogLevel.LOG_LEVEL_DEBUG);
            LoggerSystem.Instance.SaveFileLog(true);

            // DataProvider
            DataProviderSystem.Instance.RegisterDataProvider(DictionaryDataProvider.Instance);
            DataProviderSystem.Instance.RegisterDataProvider(UIWindowDataProvider.Instance);
        }

    }
}
