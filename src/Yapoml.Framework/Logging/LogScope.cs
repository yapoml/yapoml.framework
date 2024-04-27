using System;
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

    public TResult Execute<TResult>(Func<TResult> action)
    {
        TResult result;

        try
        {
            result = action();

            var task = result as Task;

            if (task is not null)
            {
                task.GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            Error = ex;
            throw;
        }

        return result;
    }

    public void Dispose()
    {
        EndTime = DateTime.Now;

        _logger.EndLogScope(this);
    }
}
