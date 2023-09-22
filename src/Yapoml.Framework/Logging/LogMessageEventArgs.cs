using System;

namespace Yapoml.Framework.Logging;

/// <summary>
/// Represents the event data for a log message.
/// </summary>
public class LogMessageEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogMessageEventArgs"/> class.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="logLevel">The log level.</param>
    /// <param name="timestamp">The timestamp of the log message.</param>
    /// <param name="logScope">The log scope this message belongs to.</param>
    public LogMessageEventArgs(string message, LogLevel logLevel, DateTimeOffset timestamp, ILogScope? logScope)
    {
        Message = message;
        LogLevel = logLevel;
        Timestamp = timestamp;
        LogScope = logScope;
    }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the log level.
    /// </summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// Gets the timestamp of the log message.
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Gets the log scope of the log message.
    /// </summary>
    public ILogScope? LogScope { get; }
}
