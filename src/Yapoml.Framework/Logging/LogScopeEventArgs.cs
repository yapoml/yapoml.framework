using System;

namespace Yapoml.Framework.Logging;

/// <summary>
/// Represents the event arguments for a log scope event.
/// </summary>
public class LogScopeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogScopeEventArgs"/> class.
    /// </summary>
    /// <param name="logScope">The log scope of the event.</param>
    /// <param name="timestamp">The timestamp of the event.</param>
    public LogScopeEventArgs(ILogScope logScope, DateTimeOffset timestamp)
    {
        LogScope = logScope;
        Timestamp = timestamp;
    }

    /// <summary>
    /// Gets the log scope of the event.
    /// </summary>
    public ILogScope LogScope { get; }

    /// <summary>
    /// Gets the timestamp of the event.
    /// </summary>
    public DateTimeOffset Timestamp { get; }
}
