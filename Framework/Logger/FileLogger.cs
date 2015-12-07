using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alkaid
{
    public class FileLogger : Logger
    {
        private object mLocker = new object();
        private const string _LogFormat = "{0}/Log_{1}.log";
        private const int _FlusInterval = 10;

        private string mSavePath;
        private FileSaver mFileSaver;
        private List<string> mMessages;
        private float mTempSeconds;

        public FileLogger()
        {
            /*base.SetDelegate(Log);*/
            mSavePath = "";
            mFileSaver = new FileSaver();
            mMessages = new List<string>();
            mTempSeconds = 0;
        }

        private void DirectWriteAll()
        {
            lock (mLocker)
            {
                foreach (var i in mMessages)
                {
                    mFileSaver.WriteLine(i);
                }
                mFileSaver.Flush();
                mMessages.Clear();
            }
        }

        public override bool Init()
        {

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
        
        public void SetSavePath(string path)
        {
            mSavePath = path;

            string name = string.Format(_LogFormat, mSavePath, DateTime.Now.ToString("yyyy-MM-dd"));
            mFileSaver.Init(name);
        }

        public override void Write(string message)
        {
            lock(mLocker)
            {
                mMessages.Add(message);
            }
        }

        /*public void Log(string message)
        {
            mMessages.Add(message);
        }*/
    }
}
