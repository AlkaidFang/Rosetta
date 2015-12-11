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
        private string mLayoutFile;
        private string mScript;

        private Dictionary<string, string> mExtraDatas;

        public IWindow()
        {
            mBaseNode = null;
            mIsShow = false;
            mIsLoad = false;
            mName = "";
            mLayoutFile = "";
            mExtraDatas = new Dictionary<string,string>();
        }

        public IWindow(string name, string layout, string script)
        {
            mBaseNode = null;
            mIsShow = false;
            mIsLoad = false;
            mName = name;
            mLayoutFile = layout;
            mScript = script;
        }

        public bool Init()
        {
            if (string.IsNullOrEmpty(mName) || string.IsNullOrEmpty(mLayoutFile))
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

        public void Load()
        {
            if (string.IsNullOrEmpty(mLayoutFile))
            {
                return;
            }

            GameObject prefab = Resources.Load(mLayoutFile) as GameObject;
            if (null == prefab)
            {
                LoggerSystem.Instance.Error("Failed to load res:" + mLayoutFile);
                return;
            }

            mBaseNode = UnityTools.AddChild(UISystem.Instance.GetRootNode(), prefab);

            mBaseNode.SetActive(false);
            mIsLoad = true;
        }

        public void UnLoad()
        {
            GameObject.Destroy(mBaseNode);
            mBaseNode = null;
            mIsLoad = false;
            Resources.UnloadUnusedAssets();
        }

        public void SetName(string name)
        {
            mName = name;
        }

        public string GetName()
        {
            return mName;
        }

        public void SetLayoutFilePath(string path)
        {
            mLayoutFile = path;
        }

        public void SetScript(string script)
        {
            mScript = script;
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
            string v = "";
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
