using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class DictionaryDataProvider : Singleton<DictionaryDataProvider>, IDataProvider
    {
        public class DictionaryData
        {
            public int mID = -1;
            public string mData = string.Empty;
        }

        private List<DictionaryData> mDataList = new List<DictionaryData>();

        public DictionaryDataProvider()
        {

        }

        public string Path()
        {
            return "data/dictionary.txt";
        }

        public void Load()
        {
            DictionaryData item = null;
            while (!FileReader.IsEnd())
            {
                FileReader.ReadLine();
                item = new DictionaryData();
                item.mID = FileReader.ReadInt();
                item.mData = FileReader.ReadString();

                mDataList.Add(item);
            }
        }

        public bool Verify()
        {
            foreach(DictionaryData i in mDataList)
	        {
                LoggerSystem.Instance.Debug("Dictionary   " + i.mID + "  " + i.mData);
	        }
	        return true;
        }
    }
}
