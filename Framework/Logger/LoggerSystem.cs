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

        private Logger mConsoleLogger;
        private Logger mFileLogger;
        private LogLevel mLogLevel;

        private bool mSaveFile;

        public LoggerSystem()
        {
            mConsoleLogger = null;
            mFileLogger = new FileLogger();
            mLogLevel = LogLevel.LOG_LEVEL_INFO;
            mSaveFile = false;
        }

        public bool Init()
        {
            mFileLogger.Init();

            return true;
        }

        public void Tick(float interval)
        {
            mFileLogger.Tick(interval);
        }

        public void Destroy()
        {
            mFileLogger.Destroy();
        }


        private void ConsoleLog(string message)
        {
            if (null != mConsoleLogger)
            {
                mConsoleLogger.Write(message);
            }
        }

        private void FileLog(string message)
        {
            if (mSaveFile && null != mFileLogger)
            {
                mFileLogger.Write(message);
            }
        }

        private void WriteLog(LogLevel level, string message)
        {
            string type = "";
            switch(level)
            {
                case LogLevel.LOG_LEVEL_DEBUG: type = "DEBUG"; break;
                case LogLevel.LOG_LEVEL_INFO: type = "INFO"; break;
                case LogLevel.LOG_LEVEL_WARN: type = "WARNING"; break;
                case LogLevel.LOG_LEVEL_ERROR: type = "ERROR"; break;
                case LogLevel.LOG_LEVEL_FATAL: type = "FATAL"; break;
            }
            message = string.Format("{0}, {1}, {2}", TimeSystem.Instance.GetFrame(), type, message);
            
            if (mLogLevel <= level)
            {
                // console log
                ConsoleLog(message);

                // file log
                FileLog(message);
            }
        }


        public void SetConsoleLogger(Logger logger)
        {
            mConsoleLogger = logger;
        }

        public void SetFileLogger(Logger logger)
        {
            mFileLogger = logger;
        }

        public void SetFileLogPath(string path)
        {
            if (mSaveFile)
            {
                ((FileLogger)mFileLogger).SetSavePath(path);
            }
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
