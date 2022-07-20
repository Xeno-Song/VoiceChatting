using log4net;
using log4net.Appender;
using log4net.Filter;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.CommonObjects.Log
{
    internal class FileLogger : ILogger
    {
        internal string FilePath
        {
            get => rollingFileAppender.File;
            set
            {
                rollingFileAppender.File = value;
                rollingFileAppender.ActivateOptions();
            }
        }

        public IAppender Appender { get => rollingFileAppender; }
        public bool IsDisposed { get; private set; } = false;

        private RollingFileAppender rollingFileAppender = null;

        internal FileLogger(string filePath)
        {
            rollingFileAppender = CreateAppender(filePath);
        }

        private RollingFileAppender CreateAppender(string filePath)
        {
            var appender = new RollingFileAppender();
            appender.Name = Path.GetFileName(filePath);
            appender.File = filePath;
            appender.Encoding = Encoding.UTF8;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.LockingModel = new RollingFileAppender.MinimalLock();
            appender.StaticLogFileName = false;
            appender.MaxSizeRollBackups = 50;
            appender.MaximumFileSize = "10MB";
            
            PatternLayout layout = new PatternLayout(
                "%date [%thread] %level - %message%newline");
            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            rollingFileAppender.Flush(100);
            rollingFileAppender.Close();
            rollingFileAppender = null;
        }
    }
}
