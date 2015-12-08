using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class ConfigSystem : Singleton<ConfigSystem>, Lifecycle
    {
        private string mConfigFilePath = "";
        private Dictionary<string, string> mConfigs = new Dictionary<string,string>();

        public ConfigSystem()
        {
            mConfigFilePath = "config/config.txt";
        }

        public bool Init()
        {
            // 加载文件
            FileReader.Load(DataProviderSystem.Instance.FormatDataProviderPath(mConfigFilePath));

            string k, v;
            while (!FileReader.IsEnd())
            {
                FileReader.ReadLine();
                FileReader.ReadInt(); // jump column
                k = FileReader.ReadString();
                v = FileReader.ReadString();

                mConfigs.Add(k.ToLowerInvariant(), v);
            }
            // 卸载文件
            FileReader.UnLoad();

            // fps
            if (mConfigs.TryGetValue("fps", out v))
            {
                FrameworkSetup.Instance.SetFPS(int.Parse(v));
            }
            // version
            if (mConfigs.TryGetValue("version", out v))
            {
                FrameworkSetup.Instance.SetVersion(v);
            }
            // log
            if (mConfigs.TryGetValue("consolelogmode", out v))
            {
                LoggerSystem.Instance.SetConsoleLogMode(int.Parse(v) > 0);
            }
            if (mConfigs.TryGetValue("consoleloglevel", out v))
            {
                LoggerSystem.Instance.SetConsoleLogLevel(int.Parse(v));
            }
            if (mConfigs.TryGetValue("filelogmode", out v))
            {
                LoggerSystem.Instance.SetFileLogMode(int.Parse(v) > 0);
            }
            if (mConfigs.TryGetValue("fileloglevel", out v))
            {
                LoggerSystem.Instance.SetFileLogLevel(int.Parse(v));
            }
            if (mConfigs.TryGetValue("filelogfrontname", out v))
            {
                LoggerSystem.Instance.SetFileLogFrontName(v);
            }
            if (mConfigs.TryGetValue("filelogextname", out v))
            {
                LoggerSystem.Instance.SetFileLogExtName(v);
            }

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
