using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class DictionaryDataProvider : Singleton<DictionaryDataProvider>, IDataProvider
    {
        public class DictionaryDataItem
        {
            public int mID = -1;
            public string mData = "";
        }

        private List<DictionaryDataItem> mDataList = new List<DictionaryDataItem>();

        public DictionaryDataProvider()
        {

        }

        public string Path()
        {
            return "data/dictionary.txt";
        }

        public void Load()
        {
            FileReader.Load(System.IO.Path.Combine(DataProviderSystem.Instance.GetRootDir(), Path()));

            DictionaryDataItem item = null;
            while (!FileReader.IsEnd())
            {
                FileReader.ReadLine();
                item = new DictionaryDataItem();
                item.mID = FileReader.ReadInt();
                item.mData = FileReader.ReadString();

                mDataList.Add(item);
            }
        }

        public bool Verify()
        {
            foreach(DictionaryDataItem i in mDataList)
	        {
                LoggerSystem.Instance.Debug("Dic   " + i.mID + "  " + i.mData);
	        }
	        return true;
        }
    }
}
