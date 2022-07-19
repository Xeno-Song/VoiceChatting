using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.Common
{
    internal class Common
    {
        internal static ILog Log { get
            {
                if (logger == null)
                    logger = CreateLogger();

                return logger;
            }
            set => logger = value;
        }
        private static ILog logger = null;

        private static ILog CreateLogger()
        {

        }
    }
}
