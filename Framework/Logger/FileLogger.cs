using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alkaid
{
    public class FileLogger : Logger
    {
        private const string _LogFormat = "{0}/{1}_{2}.{3}";
        private const int _FlusInterval = 10;

        private string mSavePath;
        private string mSaveFrontName;
        private string mSaveExtName;
        private string mFinalFilePath;
        private FileSaver mFileSaver;
        private SafeList<string> mWaitMessages;
        private float mTempSeconds;

        public FileLogger()
        {
            mSavePath = "";
            mSaveFrontName = "Log";
            mSaveExtName = "log";
            mFinalFilePath = "";

            mFileSaver = new FileSaver();
            mWaitMessages = new SafeList<string>();
            mTempSeconds = 0;
        }
        public override bool Init()
        {
            mFileSaver.Init(GetFinalFilePath());
            return true;
        }

        public override void Tick(float interval)
        {
            mTempSeconds += interval;
            if (mTempSeconds >= _FlusInterval)
            {
                mTempSeconds = 0;

                DirectWriteAll();
            }
        }

        public override void Destroy()
        {
            DirectWriteAll();

            mFileSaver.Close();
            mFileSaver = null;
        }

        public override void Write(string message)
        {
            mWaitMessages.Add(message);
        }

        private void DirectWriteAll()
        {
            mWaitMessages.Foreach((string msg) =>
            {
                mFileSaver.WriteLine(msg);
                mWaitMessages.Remove(msg);
            });

            mFileSaver.Flush();
        }
        
        public void SetSavePath(string path)
        {
            mSavePath = path;
            FormatFinalFileName();
        }
        public string GetFinalFilePath()
        {
            return mFinalFilePath;
        }

        public void SetFileLogFrontName(string name)
        {
            mSaveFrontName = name;
            FormatFinalFileName();
        }

        public void SetFileLogExtName(string name)
        {
            mSaveExtName = name;
            FormatFinalFileName();
        }
        private void FormatFinalFileName()
        {
            mFinalFilePath = string.Format(_LogFormat, mSavePath, mSaveFrontName, DateTime.Now.ToString("yyyy-MM-dd"), mSaveExtName);
        }
    }
}
