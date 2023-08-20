using System;

namespace Yapoml.Framework.Logging
{
    public class Logger : ILogger
    {
        public event EventHandler<LogMessageEventArgs> OnLogMessage;

        public void Trace(string message)
        {
            OnLogMessage?.Invoke(this, new LogMessageEventArgs(message, LogLevel.Trace, DateTimeOffset.Now));
        }
    }
}
