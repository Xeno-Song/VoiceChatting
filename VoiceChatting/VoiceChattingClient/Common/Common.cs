using VoiceChattingClient.Common.Log;

namespace VoiceChattingClient.Common
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
    }
}
