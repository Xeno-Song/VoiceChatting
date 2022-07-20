using log4net.Appender;
using log4net.Layout;
using System;

namespace VoiceChattingClient.CommonObjects.Log
{
    internal class ConsoleLogger : ILogger
    {
        public IAppender Appender { get => consoleAppender; }
        public bool IsDisposed { get; private set; } = false;
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
            if (IsDisposed) return;
            IsDisposed = true;

            consoleAppender.Flush(100);
            consoleAppender.Close();
            consoleAppender = null;
        }
    }
}
