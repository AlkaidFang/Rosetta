using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class WindowManager : Singleton<WindowManager>, Lifecycle
    {

        private Dictionary<string, IWindow> mWindowMap;
		private Dictionary<string, IWindow> mExclusiveWindows;

        public WindowManager()
        {
            mWindowMap = new Dictionary<string, IWindow>();

			mExclusiveWindows = new Dictionary<string, IWindow>();
        }

        public bool Init()
        {
            List<UIWindowDataItem> data = UIWindowDataProvider.Instance.GetAllData();
            UIWindowDataItem config;
            for (int i = 0; i < data.Count; ++i)
            {
                config = data[i];
                RegisterWindow(config.mName, config);
            }
            
            foreach (var i in mWindowMap.Values)
            {
                i.Init();
            }

            return true;
        }

        public void Tick(float interval)
        {
            foreach (var i in mWindowMap.Values)
            {
                i.Tick(interval);
            }
        }

        public void Destroy()
        {
            foreach (var i in mWindowMap.Values)
            {
                i.Destroy();
            }
        }

		private void RegisterWindow(string name, UIWindowDataItem data)
        {
			IWindow window = new IWindow(name, data);
            this.mWindowMap.Add(window.GetName(), window);
        }

        public IWindow GetWindowByName(string name)
        {
            IWindow w = null;
            mWindowMap.TryGetValue(name, out w);
            return w;
        }

        public bool IsWindowVisible(string name)
        {
            IWindow w = null;
            mWindowMap.TryGetValue(name, out w);
            if (null != w)
            {
                return w.IsShow();
            }

            return false;
        }

        public bool IsWindowLoad(string name)
        {
            IWindow w = null;
            mWindowMap.TryGetValue(name, out w);
            if (null != w)
            {
                return w.IsLoad();
            }

            return false;
        }

        public void ShowWindow(string name)
        {
            IWindow w = null;
            mWindowMap.TryGetValue(name, out w);
            if (null != w)
            {
				w.Show(true);

				CheckExclusive (w, true);
            }
        }

        public void HideWindow(string name)
        {
            IWindow w = null;
            mWindowMap.TryGetValue(name, out w);
            if (null != w)
            {
                w.Show(false); 
				w.ReleaseAssets();

				CheckExclusive (w, false);
            }
        }

        public void HideAllWindow()
        {
            foreach (var i in mWindowMap.Values)
            {
                i.Show(false);
            }
        }

		public void CheckExclusive(IWindow window, bool isShow)
		{
            /*
            if (isShow) {
				// 与windowname互斥的窗口都关闭，并且将关闭的窗口增加一个exclusive_by标签
				string ename;
				IWindow ewindow;
				for (int i = 0; i < window.GetExclusiveNames ().Count; ++i) {
					ename = window.GetExclusiveNames () [i];
					if (IsWindowVisible (ename)) {
						ewindow = GetWindowByName (ename);
						ewindow.Show (false);
						ewindow.SetExtraData ("exclusive_by", window.GetName ());
						mExclusiveWindows.Add (ewindow.GetName (), ewindow);
					}
				}
			}
			else
			{
				// 关闭了window，则之前由window导致的关闭都应该打开
				string ename;
				IWindow ewindow;
				for (int i = 0; i < window.GetExclusiveNames ().Count; ++i)
				{
					ename = window.GetExclusiveNames () [i];
					if (mExclusiveWindows.TryGetValue(ename, out ewindow))
					{
						if (ewindow.GetExtraData ("exclusive_by") == window.GetName ())
						{
							ewindow.Show (true);
							ewindow.SetExtraData ("exclusive_by", string.Empty);
							mExclusiveWindows.Remove (ename);
						}
					}
				}
			}*/
		}
    }
}
