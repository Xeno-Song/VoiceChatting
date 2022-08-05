using CommonObjects.Config;
using CommonObjects.Log;

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
    }
}
