using System;

namespace Yapoml.Framework.Logging
{
    public class LogMessageEventArgs : EventArgs
    {
        public LogMessageEventArgs(string message, LogLevel logLevel, DateTimeOffset timestamp)
        {
            Message = message;
            LogLevel = logLevel;
            Timestamp = timestamp;
        }

        public string Message { get; }

        public LogLevel LogLevel { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
