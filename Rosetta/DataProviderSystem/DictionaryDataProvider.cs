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
            public string mData = string.Empty;
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
                LoggerSystem.Instance.Debug("Dictionary   " + i.mID + "  " + i.mData);
	        }
	        return true;
        }
    }
}
