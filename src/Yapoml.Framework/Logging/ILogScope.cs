using System;

namespace Yapoml.Framework.Logging;

public interface ILogScope : IDisposable
{
    string Name { get; }

    LogLevel LogLevel { get; }

    long Depth { get; }

    ILogScope? Parent { get; }

    DateTime BeginTime { get; }

    DateTime EndTime { get; }

    ILogScope BeginScope(string name, LogLevel logLevel = LogLevel.Trace);
}
