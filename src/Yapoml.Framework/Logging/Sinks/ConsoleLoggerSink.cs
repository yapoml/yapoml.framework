using System;
using System.Collections.Generic;
using System.Linq;

namespace Yapoml.Framework.Logging.Sinks;

public class ConsoleLoggerSink : IDisposable
{
    private readonly ILogger _logger;

    private readonly IDictionary<LogLevel, string> _shortLevels = new Dictionary<LogLevel, string> {
            { LogLevel.Trace, "TRC" } };

    public ConsoleLoggerSink(ILogger logger)
    {
        _logger = logger;

        _logger.OnLogMessage += OnLogMessage;
        _logger.OnLogScopeBegin += OnLogScopeBegin;
        _logger.OnLogScopeEnd += OnLogScopeEnd;
    }

    private void OnLogMessage(object sender, LogMessageEventArgs e)
    {
        var depth = (int)(e.LogScope?.Depth + 1 ?? 0);

        var prefix = string.Concat(Enumerable.Repeat("╎ ", depth));

        Console.WriteLine($"{e.Timestamp:HH:mm:ss.fff} {_shortLevels[e.LogLevel]} {prefix}{e.Message}");
    }

    private void OnLogScopeBegin(object sender, LogScopeEventArgs e)
    {
        var depth = (int)(e.LogScope?.Depth ?? 0);

        var prefix = string.Concat(Enumerable.Repeat("╎ ", depth)) + "• ";

        Console.WriteLine($"{e.Timestamp:HH:mm:ss.fff} {_shortLevels[e.LogScope!.LogLevel]} {prefix}{e.LogScope!.Name}");
    }

    private void OnLogScopeEnd(object sender, LogScopeEventArgs e)
    {
        var duration = e.LogScope!.EndTime - e.LogScope.BeginTime;

        if (duration.TotalSeconds >= 1)
        {
            string message;

            var depth = (int)(e.LogScope?.Depth ?? 0);

            var prefix = string.Concat(Enumerable.Repeat("╎ ", depth));

            prefix += "";

            message = $"• {duration.TotalSeconds:0}s";

            Console.WriteLine($"{e.Timestamp:HH:mm:ss.fff} {_shortLevels[e.LogScope!.LogLevel]} {prefix}{message}");
        }
    }

    public void Dispose()
    {
        _logger.OnLogMessage -= OnLogMessage;
        _logger.OnLogScopeBegin -= OnLogScopeBegin;
        _logger.OnLogScopeEnd -= OnLogScopeEnd;
    }
}
