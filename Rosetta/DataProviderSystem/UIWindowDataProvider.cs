using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class UIWindowDataProvider : Singleton<UIWindowDataProvider>, IDataProvider
    {
        public class UIWindowDataItem
        {
            public int mID = -1;
            public string mName = "";
            public string mScriptName = "";
            public string mPrefabPath = "";
            public string mAltasPath = "";
            public string mParentName = "";
            public bool mUseFramework = false;
        }

        private List<UIWindowDataItem> mDataList = new List<UIWindowDataItem>();

        public string Path()
        {
            return "data/uiwindow.txt";
        }

        public void Load()
        {
            UIWindowDataItem item = null;
            while (!FileReader.IsEnd())
            {
                FileReader.ReadLine();
                item = new UIWindowDataItem();
                item.mID = FileReader.ReadInt();
                item.mName = FileReader.ReadString();
                item.mScriptName = FileReader.ReadString();
                item.mPrefabPath = FileReader.ReadString();
                item.mAltasPath = FileReader.ReadString();
                item.mParentName = FileReader.ReadString();
                item.mUseFramework = FileReader.ReadBoolean();

                mDataList.Add(item);
            }
        }

        public bool Verify()
        {
            foreach(var i in mDataList)
	        {
                LoggerSystem.Instance.Debug("UIWindow   " + i.mID + "  " + i.mName);
	        }
	        return true;
        }
    }
}
