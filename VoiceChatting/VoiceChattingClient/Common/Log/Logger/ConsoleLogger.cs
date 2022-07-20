using log4net.Appender;
using log4net.Layout;
using System;

namespace VoiceChattingClient.Common.Log
{
    internal class ConsoleLogger : ILogger, IDisposable
    {
        public IAppender Appender { get => consoleAppender; }
        private ConsoleAppender consoleAppender = null;

        internal ConsoleLogger()
        {
            consoleAppender = CreateAppender();
        }

        private ConsoleAppender CreateAppender()
        {
            var consoleAppender = new ConsoleAppender();

            PatternLayout layout = new PatternLayout("%date %level - %message%newline");
            consoleAppender.Layout = layout;
            consoleAppender.ActivateOptions();

            return consoleAppender;
        }

        public void Dispose()
        {
            consoleAppender.Flush(100);
            consoleAppender.Close();
            consoleAppender = null;
        }
    }
}
