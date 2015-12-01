using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class LoggerSystem : InstanceTemplate<LoggerSystem>, Lifecycle
    {
        public enum LogLevel
        {
            Info = 1,
            Debug = 2,
            Warn = 3,
            Error = 4,
            Fatal = 5,
        }

        private Callback<string> mOutPut = UnityEngine.Debug.Log;
        private LogLevel mLogLevel;

        public LoggerSystem()
        {

        }

        public bool Init()
        {

            return true;
        }

        public void Tick()
        {

        }

        public void Destroy()
        {

        }

        public void SetDelegate(Callback<string> output)
        {
            mOutPut = output;
        }

        public void SetLogLevel(int level)
        {
            mLogLevel = (LogLevel)level;
        }

        public void Info(string message)
        {
            if (null != mOutPut && mLogLevel <= LogLevel.Info)
            {
                mOutPut("[info]" + message);
            }
        }

        public void Debug(string message)
        {
            if (null != mOutPut && mLogLevel <= LogLevel.Debug)
            {
                mOutPut("[debug]" + message);
            }
        }

        public void Warn(string message)
        {
            if (null != mOutPut && mLogLevel <= LogLevel.Warn)
            {
                mOutPut("[warn]" + message);
            }
        }

        public void Error(string message)
        {
            if (null != mOutPut && mLogLevel <= LogLevel.Error)
            {
                mOutPut("[error]" + message);
            }
        }

        public void Fatal(string message)
        {
            if (null != mOutPut && mLogLevel <= LogLevel.Fatal)
            {
                mOutPut("[fatal]" + message);
            }
        }
    }
}
