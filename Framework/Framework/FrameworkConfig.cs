using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class FrameworkConfig : InstanceTemplate<FrameworkConfig>
    {
        private int mFPS = 30;

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

        /**
         * 设置logsystem的日志输出委托
         * */
        public void SetLoggerSystemDelegate(Callback<string> output)
        {
            LoggerSystem.Instance.SetDelegate(output);
        }

        public void SetLoggerSystemLevel(int level)
        {
            LoggerSystem.Instance.SetLogLevel(level);
        }

        /**
         * 设置文件读取是否从数据库
         * */
        public void SetDataProviderFromDB(bool fromdb)
        {

        }

        /**
         * 设置文件读取是否加密
         * */
        public void SetDataProviderEncryptioned(bool encryptioned)
        {

        }
    }
}
