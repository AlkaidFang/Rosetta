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
            LOG_LEVEL_ALWAYS = 6,
        }

        private bool mConsoleLogMode;
        private Logger mConsoleLogger;
        private LogLevel mConsoleLogLevel;
        private bool mFileLogMode;
        private Logger mFileLogger;
        private LogLevel mFileLogLevel;


        public LoggerSystem()
        {
            mConsoleLogMode = true;
            mConsoleLogger = null;
            mConsoleLogLevel = LogLevel.LOG_LEVEL_INFO;

            mFileLogMode = true;
            mFileLogger = new FileLogger();
            mFileLogLevel = LogLevel.LOG_LEVEL_INFO;
        }

        public bool Init()
        {
            mFileLogger.Init();
            ConsoleLog(LogLevel.LOG_LEVEL_ALWAYS, "FileLogger file path:" + (mFileLogger as FileLogger).GetFinalFilePath());

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
            
            // console log
            ConsoleLog(level, message);

            // file log
            FileLog(level, message);
        }

        private void ConsoleLog(LogLevel level, string message)
        {
            if (mConsoleLogMode && mConsoleLogLevel <= level && null != mConsoleLogger)
            {
                mConsoleLogger.Write(message);
            }
        }

        private void FileLog(LogLevel level, string message)
        {
            if (mFileLogMode && mFileLogLevel <= level && null != mFileLogger)
            {
                mFileLogger.Write(message);
            }
        }

        /**
         * 设置console log的方法
         * */
        public void SetConsoleLogMode(bool status)
        {
            mConsoleLogMode = status;
        }
        public void SetConsoleLogger(Logger logger)
        {
            mConsoleLogger = logger;
        }
        public void SetConsoleLogLevel(int level)
        {
            mConsoleLogLevel = (LogLevel)level;
        }

        /**
         * 设置file log的方法
         * */
        public void SetFileLogMode(bool status)
        {
            mFileLogMode = status;
        }
        public void SetFileLogger(Logger logger)
        {
            mFileLogger = logger;
        }
        public void SetFileLogLevel(int level)
        {
            mFileLogLevel = (LogLevel)level;
        }
        public void SetFileLogPath(string path)
        {
            if (mFileLogMode)
            {
                ((FileLogger)mFileLogger).SetSavePath(path);
            }
        }
        public void SetFileLogFrontName(string name)
        {
            ((FileLogger)mFileLogger).SetFileLogFrontName(name);
        }
        public void SetFileLogExtName(string name)
        {
            ((FileLogger)mFileLogger).SetFileLogExtName(name);
        }
        
    }
}
