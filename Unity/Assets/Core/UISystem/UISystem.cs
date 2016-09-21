using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public class UISystem : Singleton<UISystem>, Lifecycle
    {
        private GameObject mUIRoot;
        private GameObject mUICamera;
        private GameObject mForwardCamera;

        public UISystem()
        {
            mUIRoot = null;
        }

        public bool Init()
        {
            mUIRoot = UnityEngine.GameObject.Find("UIRoot");
            if (mUIRoot == null)
            {
                return false;
            }

            mUICamera = mUIRoot.transform.FindChild("UICamera").gameObject;
            mForwardCamera = mUIRoot.transform.FindChild("ForwardCamera").gameObject;
            
            // 设置UISystem的一些数据
            UnityEngine.Object.DontDestroyOnLoad(mUIRoot);

            WindowManager.Instance.Init();

            return true;
        }

        public void Tick(float interval)
        {
            WindowManager.Instance.Tick(interval);
        }

        public void Destroy()
        {
            WindowManager.Instance.Destroy();
        }

        public GameObject GetUICamera()
        {
            return mUICamera;
        }

        public GameObject GetForwardCamera()
        {
            return mForwardCamera;
        }
    }
}
