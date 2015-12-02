using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class FrameworkSetup : Singleton<FrameworkSetup>
    {
        private int mFPS = 30;
        private Callback mSetupFromProject = null;
        private Callback mSetupFromUnity = null;

        public void SetFPS(int fps)
        {
            if (fps > 0)
            {
                mFPS = fps;
            }
        }

        public int GetFPS()
        {
            return mFPS;
        }

        /**
         * 是否清空UI缓存
         * */
        public void SetClearUICache(bool clear)
        {

        }

        /**
         * 是否在运行前检查更新
         * */
        public void SetCheckUpdateBeforeRun(bool check)
        {

        }

        /**
         * 是否在运行前下载资源
         * */
        public void SetDownloadResBeforeRun(bool downloadres)
        {

        }

        public void RegisterSetupFromUnity(Callback fromUnity)
        {
            mSetupFromUnity = fromUnity;
        }
        public void RegisterSetupFromPorject(Callback fromProject)
        {
            mSetupFromProject = fromProject;
        }

        public void Apply()
        {
            if (mSetupFromUnity != null)
            {
                mSetupFromUnity();
            }

            if (mSetupFromProject != null)
            {
                mSetupFromProject();
            }
        }
    }
}
