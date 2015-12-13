using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class WindowManager : Singleton<WindowManager>, Lifecycle
    {

        private Dictionary<string, IWindow> mWindowMap;

        public WindowManager()
        {
            mWindowMap = new Dictionary<string, IWindow>();


        }

        public bool Init()
        {
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

        public void RegisterWindow(string name, string layoutFile, string scriptName)
        {
            IWindow window = new IWindow(name, layoutFile, scriptName);
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
            }
        }

        public void HideAllWindow()
        {
            foreach (var i in mWindowMap.Values)
            {
                i.Show(false);
            }
        }
    }
}
