using System;

namespace Yapoml.Framework.Logging
{
    public class LogMessageEventArgs : EventArgs
    {
        public LogMessageEventArgs(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }

        public string Message { get; }

        public LogLevel LogLevel { get; }
    }
}
