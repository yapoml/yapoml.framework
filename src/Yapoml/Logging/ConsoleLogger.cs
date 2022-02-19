using System;
using System.Collections.Generic;

namespace Yapoml.Logging
{
    public class ConsoleLogger : ILogger
    {
        private readonly IDictionary<LogLevel, string> _shortLevels = new Dictionary<LogLevel, string> {
                { LogLevel.Trace, "TRC" },
                { LogLevel.Info, "INF" }
            };

        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        private void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {_shortLevels[level]} {message}");
        }
    }
}
