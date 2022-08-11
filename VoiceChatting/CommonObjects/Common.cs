using CommonObjects.Config;
using CommonObjects.Log;
using System.ServiceModel.Channels;

namespace CommonObjects
{
    internal class Common
    {
        private static Logging log = null;
        internal static Logging Log
        {
            get
            {
                if (log == null)
                    log = new Logging();
                return log;
            }
            private set => log = value;
        }

        private static ConfigIndexer config = null;
        internal static ConfigIndexer Config
        {
            get
            {
                if (config == null) config = new ConfigIndexer();
                return config;
            }
            private set => config = value;
        }

        private static BufferManager bufferManager = null;
        internal static BufferManager BufferManager
        {
            get
            {
                if (bufferManager == null) bufferManager = BufferManager.CreateBufferManager(100, 1024);
                return bufferManager;
            }
            private set => bufferManager = value;
        }
    }
}
