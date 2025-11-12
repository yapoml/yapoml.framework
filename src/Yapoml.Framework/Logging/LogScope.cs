using System;
using System.Threading;
using System.Threading.Tasks;

namespace Yapoml.Framework.Logging;

internal class LogScope : ILogScope
{
    private readonly ILogger _logger;

    public LogScope(ILogger logger, ILogScope? logScope, string name, LogLevel logLevel)
    {
        _logger = logger;
        Parent = logScope;
        Name = name;
        LogLevel = logLevel;

        Depth = logScope?.Depth + 1 ?? 0;

        BeginTime = DateTime.Now;
    }

    public string Name { get; }

    public LogLevel LogLevel { get; }

    public long Depth { get; }

    public ILogScope? Parent { get; }

    public DateTime BeginTime { get; }

    public DateTime EndTime { get; private set; }

    public Exception Error { get; private set; }

    public ILogScope BeginScope(string name, LogLevel logLevel = LogLevel.Trace)
    {
        var logScope = new LogScope(_logger, this, name, logLevel);

        return _logger.BeginLogScope(logScope);
    }

    public void Execute(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Error = ex;
            throw;
        }
    }

    public void Execute(Func<Task> func)
    {
        try
        {
            if (SynchronizationContext.Current is null && TaskScheduler.Current == TaskScheduler.Default)
            {
                func().GetAwaiter().GetResult();
            }
            else
            {
                Task.Run(func).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            Error = ex;
            throw;
        }
    }

    public TResult Execute<TResult>(Func<TResult> func)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            Error = ex;
            throw;
        }
    }

    public TResult Execute<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            if (SynchronizationContext.Current is null && TaskScheduler.Current == TaskScheduler.Default)
            {
                return func().GetAwaiter().GetResult();
            }
            else
            {
                return Task.Run(func).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            Error = ex;
            throw;
        }
    }

    public void Dispose()
    {
        EndTime = DateTime.Now;

        _logger.EndLogScope(this);
    }
}
