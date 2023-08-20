using System;
using System.Collections.Generic;

namespace Yapoml.Framework.Logging.Listeners
{
    public class ConsoleLogListener : ILogListener, IDisposable
    {
        private ILogger _logger;

        private readonly IDictionary<LogLevel, string> _shortLevels = new Dictionary<LogLevel, string> {
                { LogLevel.Trace, "TRC" } };

        public void Initialize(ILogger logger)
        {
            _logger = logger;

            _logger.OnLogMessage += OnLogMessage;
        }

        private void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {_shortLevels[e.LogLevel]} {e.Message}");
        }

        public void Dispose()
        {
            _logger.OnLogMessage -= OnLogMessage;
        }
    }
}
