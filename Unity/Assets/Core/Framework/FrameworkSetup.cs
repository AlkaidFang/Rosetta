using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class FrameworkSetup : Singleton<FrameworkSetup>
    {
        private int mFPS = 30;
        private string mVersion = string.Empty;
        private string mStreamAssetsRootDir = string.Empty;
		private string mWritableRootDir = string.Empty;

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

        public void SetVersion(string version)
        {
            mVersion = version;
            LocalStorageSystem.Instance.SetAppVersion(mVersion);
        }

        public string GetVersion()
        {
            return mVersion;
        }

        public void SetStreamAssetsRootDir(string path)
        {
            mStreamAssetsRootDir = path;
        }

        public string GetStreamAssetsRootDir()
        {
            return mStreamAssetsRootDir;
        }

		public void SetWritableRootDir(string path)
		{
			mWritableRootDir = path;
		}

		public string GetWritableRootDir()
		{
			return mWritableRootDir;
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

    }
}
