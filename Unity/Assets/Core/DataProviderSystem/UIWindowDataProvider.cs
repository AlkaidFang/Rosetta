using System;
using System.Collections.Generic;

namespace Alkaid
{
    public class UIWindowDataProvider : Singleton<UIWindowDataProvider>, IDataProvider
    {
        public class UIWindowData
        {
            public int mID = -1;
            public string mName = string.Empty;
            public string mScriptName = string.Empty;
            public string mPrefabPath = string.Empty;
            public string mAltasPath = string.Empty;
            public string mParentName = string.Empty;
            public bool mUseFramework = false;
			public string mExclusiveIDs = string.Empty;

        }

		public Dictionary<int, UIWindowData> mDataMap = new Dictionary<int, UIWindowData>();

        public string Path()
        {
            return "data/uiwindow.txt";
        }

        public void Load()
        {
            UIWindowData item = null;
            while (!FileReader.IsEnd())
            {
                FileReader.ReadLine();
                item = new UIWindowData();
                item.mID = FileReader.ReadInt();
                item.mName = FileReader.ReadString();
                item.mScriptName = FileReader.ReadString();
                item.mPrefabPath = FileReader.ReadString();
                item.mAltasPath = FileReader.ReadString();
                item.mParentName = FileReader.ReadString();
                item.mUseFramework = FileReader.ReadBoolean();
				item.mExclusiveIDs = FileReader.ReadString();

				mDataMap.Add (item.mID, item);
            }

        }

        public bool Verify()
        {
			foreach (var i in mDataMap.Values)
	        {
                LoggerSystem.Instance.Debug("UIWindow   " + i.mID + "  " + i.mName);
	        }
	        return true;
        }
    }
}
