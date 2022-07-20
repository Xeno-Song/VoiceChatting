using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChattingClient.CommonObjects.Log.Logger;

namespace VoiceChattingClient.CommonObjects.Log
{
    internal class Logging : IDisposable
    {
        internal ILog this[string key]
        {
            get => LogManager.Exists(key);
        }

        private Dictionary<string, ILogger> loggers = null;

        internal Logging()
        {
            loggers = new Dictionary<string, ILogger>();
        }

        public bool CreateLogger(string key, string logPath)
        {
            // If logging directory not exist
            if (Directory.Exists(Path.GetDirectoryName(logPath)) == false)
                return false;

            ILog log = LogManager.GetLogger(key);
            var logger = (log4net.Repository.Hierarchy.Logger)log.Logger;
            logger.Level = log4net.Core.Level.All;
            logger.Repository.Configured = true;

            var consoleLogger = new ConsoleLogger();
            var fileLogger = new FileLogger(logPath);
            var asyncForwarder = new AsyncForwarder();

            asyncForwarder.AddAppender(consoleLogger);
            asyncForwarder.AddAppender(fileLogger);

            loggers.Add(key, asyncForwarder);
            return true;
        }

        public void Dispose()
        {
            foreach (var loggerPair in loggers)
            {
                var logger = LogManager.Exists(loggerPair.Key);
                if (logger != null) logger.Logger.Repository.Shutdown();

                loggerPair.Value.Dispose();
            }
        }
    }
}
