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
            DEBUG = 1,
            INFO = 2,
            WARN = 3,
            ERROR = 4,
            FATAL = 5,
            ALWAYS = 6,
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
            mConsoleLogLevel = LogLevel.INFO;

            mFileLogMode = true;
            mFileLogger = new FileLogger();
            mFileLogLevel = LogLevel.INFO;
        }

        public bool Init()
        {
            mFileLogger.Init();
            ConsoleLog(LogLevel.ALWAYS, "FileLogger file path:" + (mFileLogger as FileLogger).GetFinalFilePath());

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
            WriteLog(LogLevel.DEBUG, message);
        }

        public void Info(string message)
        {
            WriteLog(LogLevel.INFO, message);
        }

        public void Warn(string message)
        {
            WriteLog(LogLevel.WARN, message);
        }

        public void Error(string message)
        {
            WriteLog(LogLevel.ERROR, message);
            WriteLog(LogLevel.ERROR, UtilTools.GetCallStack());
        }

        public void Fatal(string message)
        {
            WriteLog(LogLevel.FATAL, message);
            WriteLog(LogLevel.FATAL, UtilTools.GetCallStack());
        }
        private void WriteLog(LogLevel level, string message)
        {
            string type = "";
            switch(level)
            {
                case LogLevel.DEBUG: type = "DEBUG"; break;
                case LogLevel.INFO: type = "INFO"; break;
                case LogLevel.WARN: type = "WARNING"; break;
                case LogLevel.ERROR: type = "ERROR"; break;
                case LogLevel.FATAL: type = "FATAL"; break;
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
