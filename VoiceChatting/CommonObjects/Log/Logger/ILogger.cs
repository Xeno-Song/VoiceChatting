using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonObjects.Log
{
    internal interface ILogger : IDisposable
    {
        IAppender Appender { get; }
        bool IsDisposed { get; }
    }
}
