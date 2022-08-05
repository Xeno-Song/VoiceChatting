using log4net.Appender;
using Log4Net.Async;
using System.Collections.Generic;

namespace CommonObjects.Log.Logger
{
    internal class AsyncForwarder : ILogger
    {
        public IAppender Appender { get => asyncForwardingAppender; }
        public IReadOnlyList<ILogger> FowardingAppenders
        {
            get => forwardingAppenders.AsReadOnly();
        }
        public bool IsDisposed { get; private set; } = false;

        private AsyncForwardingAppender asyncForwardingAppender = null;
        private List<ILogger> forwardingAppenders;

        internal AsyncForwarder()
        {
            forwardingAppenders = new List<ILogger>();
            CreateAppender();
        }

        private void CreateAppender()
        {
            asyncForwardingAppender = new AsyncForwardingAppender();
            asyncForwardingAppender.ActivateOptions();
        }

        public bool AddAppender(ILogger logger)
        {
            if (logger == null) return false;
            if (logger.Appender == null) return false;

            forwardingAppenders.Add(logger);
            asyncForwardingAppender.AddAppender(logger.Appender);

            return true;
        }

        public bool RemoveAppender(ILogger logger)
        {
            if (logger == null) return false;
            if (logger.Appender == null) return false;
            if (forwardingAppenders.Contains(logger) == false) return false;

            asyncForwardingAppender.RemoveAppender(logger.Appender);
            forwardingAppenders.Remove(logger);

            return true;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            asyncForwardingAppender.Flush(100);
            asyncForwardingAppender.RemoveAllAppenders();
            asyncForwardingAppender.Close();

            foreach (var appender in forwardingAppenders)
                appender.Dispose();
        }
    }
}
