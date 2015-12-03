using System;
using System.Collections.Generic;
using System.Text;

namespace Alkaid
{
    public class DataProviderSystem : Singleton<DataProviderSystem>, Lifecycle
    {
        private string mDir = "";

        private List<IDataProvider> mDataProvider = new List<IDataProvider>();

        public bool Init()
        {
            LoggerSystem.Instance.Info("DataProviderSystem   init   begin");
            IDataProvider provider = null;
            for (int i = 0; i < mDataProvider.Count; ++i)
            {
                provider = mDataProvider[i];
                if (provider != null)
                {
                    provider.Load();
                    if (!provider.Verify()) return false;
                }
            }

            LoggerSystem.Instance.Info("DataProviderSystem   init   end");
            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {
            mDataProvider.Clear();
        }

        public void RegisterDataProvider(IDataProvider dataProvider)
        {
            mDataProvider.Add(dataProvider);
        }
        
        public void SetRootDir(string dir)
        {
            mDir = dir;
        }

        public string GetRootDir()
        {
            return mDir;
        }
    }
}
