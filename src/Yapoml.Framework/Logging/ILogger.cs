using System;

namespace Yapoml.Framework.Logging;

/// <summary>
/// Defines the contract for a logger component.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Begins a new log scope with the specified name.
    /// </summary>
    /// <param name="name">The name of the log scope.</param>
    /// <param name="logLevel">The log level of the log scope.</param>
    /// <returns>An <see cref="ILogScope"/> instance that represents the log scope.</returns>
    ILogScope BeginLogScope(string name, LogLevel logLevel = LogLevel.Trace);

    /// <summary>
    /// Begins a new log scope with the specified parent log scope.
    /// </summary>
    /// <param name="logScope">The parent log scope.</param>
    /// <returns>An <see cref="ILogScope"/> instance that represents the log scope.</returns>
    ILogScope BeginLogScope(ILogScope logScope);

    /// <summary>
    /// Ends the specified log scope.
    /// </summary>
    /// <param name="logScope">The log scope to end.</param>
    void EndLogScope(ILogScope logScope);

    /// <summary>
    /// Writes a trace message to the logger.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void Trace(string message);

    /// <summary>
    /// Occurs when a log message is written to the logger.
    /// </summary>
    event EventHandler<LogMessageEventArgs> OnLogMessage;

    /// <summary>
    /// Occurs when a log scope is started.
    /// </summary>
    event EventHandler<LogScopeEventArgs> OnLogScopeBegin;

    /// <summary>
    /// Occurs when a log scope is ended.
    /// </summary>
    event EventHandler<LogScopeEventArgs> OnLogScopeEnd;
}
