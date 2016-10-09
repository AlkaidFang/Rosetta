using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public class IWindow : Lifecycle
    {
        private GameObject mBaseNode;
        private bool mIsShow;
        private bool mIsLoad;
        private string mName;
        private UIWindowData mData;

        private Dictionary<string, string> mExtraDatas;

        public IWindow()
        {
            mBaseNode = null;
            mIsShow = false;
            mIsLoad = false;
            mName = string.Empty;
            mExtraDatas = new Dictionary<string,string>();
        }

		public IWindow(string name, UIWindowData data)
        {
            mBaseNode = null;
            mIsShow = false;
            mIsLoad = false;
            mName = name;
            mData = data;
			mExtraDatas = new Dictionary<string,string>();
        }

        public bool Init()
        {
            if (string.IsNullOrEmpty(mName) || string.IsNullOrEmpty(mData.mPrefabPath))
            {
                LoggerSystem.Instance.Error("IWindow   init   failed! name:" + mName);
            }
            else
            {
                LoggerSystem.Instance.Info("IWindow   init   successed! name:" + mName);
            }

            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {
            UnLoad();
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(mData.mPrefabPath))
            {
                return;
            }

            GameObject prefab = Resources.Load(mData.mPrefabPath) as GameObject;
            if (null == prefab)
            {
                LoggerSystem.Instance.Error("Failed to load res:" + mData.mPrefabPath);
                return;
            }

            mBaseNode = UnityTools.AddChild(UISystem.Instance.GetUICamera(), prefab);

            mBaseNode.SetActive(false);
            mIsLoad = true;
        }

        private void UnLoad()
        {
            if (mIsLoad)
            {
                GameObject.Destroy(mBaseNode);
                mBaseNode = null;
                mIsLoad = false;
                Resources.UnloadUnusedAssets();
            }
        }

        public void Show(bool visible)
        {
            if (this.mIsShow == visible)
            {
                return;
            }
            else
            {
                if (this.mIsLoad == false)
                {
                    this.Load();
                }
            }

            this.mIsShow = visible;
            if (null != this.mBaseNode)
            {
                this.mBaseNode.SetActive(visible);
            }
        }

        public void ReleaseAssets()
        {
            UnLoad();
        }

        public void SetName(string name)
        {
            mName = name;
        }

        public string GetName()
        {
            return mName;
        }

        public UIWindowData GetConfig()
        {
            return mData;
        }

        public void SetExtraData(string k, string v)
        {
            if (mExtraDatas.ContainsKey(k))
            {
                mExtraDatas[k] = v;
            }
            else
            {
                mExtraDatas.Add(k, v);
            }
        }

        public string GetExtraData(string k)
        {
            string v = string.Empty;
            mExtraDatas.TryGetValue(k, out v);
            return v;
        }

        public bool IsShow()
        {
            return mIsShow;
        }

        public bool IsLoad()
        {
            return mIsLoad;
        }

    }
}
