using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.Common.Log
{
    internal interface ILogger
    {
        IAppender Appender { get; }
    }
}
