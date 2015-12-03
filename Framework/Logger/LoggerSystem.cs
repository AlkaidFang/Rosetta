using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class LoggerSystem : Singleton<LoggerSystem>, Lifecycle
    {
        public enum LogLevel
        {
            LOG_LEVEL_DEBUG = 1,
            LOG_LEVEL_INFO = 2,
            LOG_LEVEL_WARN = 3,
            LOG_LEVEL_ERROR = 4,
            LOG_LEVEL_FATAL = 5,
        }

        private Callback<string> mConsoleOutput = null;
        private Callback<string> mFileOutput = null;
        private LogLevel mLogLevel = LogLevel.LOG_LEVEL_INFO;

        private bool mSaveFile = false;

        public LoggerSystem()
        {
            mConsoleOutput = null;
            mFileOutput = null;
            mLogLevel = LogLevel.LOG_LEVEL_INFO;
        }

        public bool Init()
        {

            return true;
        }

        public void Tick(float interval)
        {

        }

        public void Destroy()
        {

        }


        private void ConsoleLog(string message)
        {
            if (null != mConsoleOutput)
            {
                mConsoleOutput(message);
            }
        }

        private void FileLog(string message)
        {
            if (mSaveFile && null != mFileOutput)
            {
                mFileOutput(message);
            }
        }

        private void WriteLog(LogLevel level, string message)
        {
            string msg = "";
            switch(level)
            {
                case LogLevel.LOG_LEVEL_DEBUG: msg = "DEBUG:"; break;
                case LogLevel.LOG_LEVEL_INFO: msg = "INFO:"; break;
                case LogLevel.LOG_LEVEL_WARN: msg = "WARNING:"; break;
                case LogLevel.LOG_LEVEL_ERROR: msg = "ERROR:"; break;
                case LogLevel.LOG_LEVEL_FATAL: msg = "FATAL:"; break;
            }
            msg = msg + " " + message;
            
            if (mLogLevel <= level)
            {
                // console log
                ConsoleLog(msg);

                // file log
                FileLog(msg);
            }
        }


        public void SetConsoleDelegate(Callback<string> output)
        {
            mConsoleOutput = output;
        }

        public void SetFileDelegate(Callback<string> output)
        {
            mFileOutput = output;
        }

        public void SaveFileLog(bool status)
        {
            mSaveFile = status;
        }

        public void SetLogLevel(int level)
        {
            mLogLevel = (LogLevel)level;
        }

        public void Debug(string message)
        {
            WriteLog(LogLevel.LOG_LEVEL_DEBUG, message);
        }

        public void Info(string message)
        {
            WriteLog(LogLevel.LOG_LEVEL_INFO, message);
        }

        public void Warn(string message)
        {
            WriteLog(LogLevel.LOG_LEVEL_WARN, message);
        }

        public void Error(string message)
        {
            WriteLog(LogLevel.LOG_LEVEL_ERROR, message);
        }

        public void Fatal(string message)
        {
            WriteLog(LogLevel.LOG_LEVEL_FATAL, message);
        }
        
    }
}
