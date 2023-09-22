using System;
using System.Threading;

namespace Yapoml.Framework.Logging;

/// <inheritdoc />
/// <summary>
/// Provides a default implementation of <see cref="ILogger"/>.
/// </summary>
public class Logger : ILogger
{
    private readonly AsyncLocal<ILogScope?> _currentLogScope = new();

    /// <inheritdoc/>
    public event EventHandler<LogMessageEventArgs>? OnLogMessage;

    /// <inheritdoc/>
    public event EventHandler<LogScopeEventArgs>? OnLogScopeBegin;

    /// <inheritdoc/>
    public event EventHandler<LogScopeEventArgs>? OnLogScopeEnd;

    /// <inheritdoc/>
    public ILogScope BeginLogScope(string name, LogLevel logLevel = LogLevel.Trace)
    {
        var logScope = new LogScope(this, null, name, logLevel);

        _currentLogScope.Value = logScope;

        OnLogScopeBegin?.Invoke(this, new LogScopeEventArgs(logScope, DateTime.Now));

        return logScope;
    }

    /// <inheritdoc/>
    public ILogScope BeginLogScope(ILogScope logScope)
    {
        _currentLogScope.Value = logScope;

        OnLogScopeBegin?.Invoke(this, new LogScopeEventArgs(logScope, DateTime.Now));

        return logScope;
    }

    /// <inheritdoc/>
    public void EndLogScope(ILogScope logScope)
    {
        _currentLogScope.Value = logScope.Parent;

        OnLogScopeEnd?.Invoke(this, new LogScopeEventArgs(logScope, DateTime.Now));
    }

    /// <inheritdoc/>
    public void Trace(string message)
    {
        OnLogMessage?.Invoke(this, new LogMessageEventArgs(message, LogLevel.Trace, DateTime.Now, _currentLogScope.Value));
    }
}
