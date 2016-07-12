using System;
using System.Collections.Generic;
using System.Text;

namespace Alkaid
{
    public class DataProviderSystem : Singleton<DataProviderSystem>, Lifecycle
    {
        private List<IDataProvider> mDataProvider = new List<IDataProvider>();

        public bool Init()
        {
            LoggerSystem.Instance.Info("DataProviderSystem   init   begin");
            IDataProvider provider = null;
            for (int i = 0; i < mDataProvider.Count; ++i)
            {
                provider = mDataProvider[i];
                if (null != provider)
                {
                    FileReader.Load(FormatDataProviderPath(provider.Path()));

                    provider.Load();

                    if (!provider.Verify()) return false;

                    FileReader.UnLoad();
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

        public string FormatDataProviderPath(string datapath)
        {
            return System.IO.Path.Combine(Framework.Instance.GetStreamAssetsRootDir(), datapath);
        }
    }
}
