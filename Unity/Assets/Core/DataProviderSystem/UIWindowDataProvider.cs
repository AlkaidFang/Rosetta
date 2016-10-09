using System;
using System.Collections.Generic;

namespace Alkaid
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
		public bool mHideWithDestroy = true;

    }

    public class UIWindowDataProvider : Singleton<UIWindowDataProvider>, IDataProvider
    {
        private List<UIWindowData> mDataList = new List<UIWindowData>();

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

                mDataList.Add(item);
            }

            /*// 加入注册所有窗口
			List<string> exclusive = new List<string>();
			List<int> eid;
            foreach (var i in mDataList)
            {
				exclusive.Clear ();
				eid = Converter.ConvertNumberList<int> (i.mExclusiveIDs);
				foreach (var id in eid)
				{
					foreach (var j in mDataList)
					{
						if (id == j.mID)
						{
							exclusive.Add (j.mName);
						}
					}	
				}

				WindowManager.Instance.RegisterWindow(i.mName, i.mPrefabPath, i.mScriptName, exclusive);
            }*/
        }

        public bool Verify()
        {
            foreach(var i in mDataList)
	        {
                LoggerSystem.Instance.Debug("UIWindow   " + i.mID + "  " + i.mName);
	        }
	        return true;
        }

        public List<UIWindowData> GetAllData()
        {
            return mDataList;
        }

		public UIWindowData GetData(string name)
		{
			UIWindowData ret = null;
			for (int i = 0; i < mDataList.Count; ++i) {
				if (mDataList [i].mName.Equals (name))
					ret = mDataList [i];
			}

			return ret;
		}
    }
}
