using System;
using System.Threading.Tasks;

namespace Yapoml.Framework.Logging;

public interface ILogScope : IDisposable
{
    string Name { get; }

    LogLevel LogLevel { get; }

    long Depth { get; }

    ILogScope? Parent { get; }

    DateTime BeginTime { get; }

    DateTime EndTime { get; }

    Exception Error { get; }

    ILogScope BeginScope(string name, LogLevel logLevel = LogLevel.Trace);

    void Execute(Action action);

    void Execute(Func<Task> func);
}
