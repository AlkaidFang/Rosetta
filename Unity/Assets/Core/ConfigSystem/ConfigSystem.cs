using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alkaid
{
    public class ConfigSystem : Singleton<ConfigSystem>, Lifecycle
    {
        private string mConfigFilePath = string.Empty;
        private Dictionary<string, string> mConfigs = new Dictionary<string,string>();

        public ConfigSystem()
        {
            mConfigFilePath = "config/config.txt";
        }

        public bool Init()
        {
            // 加载文件
            FileReader.LoadPath(DataProviderSystem.Instance.FormatDataProviderPath(mConfigFilePath));

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

            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {

        }

        public bool TryGetConfig(string key, out string v)
        {
            return mConfigs.TryGetValue(key, out v);
        }
    }
}
