using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public class UISystem : Singleton<UISystem>, Lifecycle
    {
        private GameObject mUIRoot;

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

        public GameObject GetRootNode()
        {
            return mUIRoot;
        }

    }
}
