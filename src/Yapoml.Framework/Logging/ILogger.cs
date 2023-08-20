using System;

namespace Yapoml.Framework.Logging
{
    public interface ILogger
    {
        void Trace(string message);

        event EventHandler<LogMessageEventArgs> OnLogMessage;
    }
}
