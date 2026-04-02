using System;
using System.Threading.Tasks;

namespace Yapoml.Framework.Logging;

/// <summary>
/// Represents a hierarchical logging scope that tracks execution timing and errors.
/// </summary>
public interface ILogScope : IDisposable
{
    /// <summary>
    /// Gets the name of the log scope.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the log level of the scope.
    /// </summary>
    LogLevel LogLevel { get; }

    /// <summary>
    /// Gets the nesting depth of the scope. Zero-based.
    /// </summary>
    long Depth { get; }

    /// <summary>
    /// Gets the parent log scope, or <see langword="null"/> if this is a root scope.
    /// </summary>
    ILogScope? Parent { get; }

    /// <summary>
    /// Gets the timestamp when the scope was created.
    /// </summary>
    DateTimeOffset BeginTime { get; }

    /// <summary>
    /// Gets the timestamp when the scope ended.
    /// </summary>
    DateTimeOffset EndTime { get; }

    /// <summary>
    /// Gets the exception that occurred during scope execution, or <see langword="null"/> if no error occurred.
    /// </summary>
    Exception Error { get; }

    /// <summary>
    /// Begins a new child log scope with the specified name.
    /// </summary>
    /// <param name="name">The name of the child scope.</param>
    /// <param name="logLevel">The log level for the child scope.</param>
    /// <returns>The new child <see cref="ILogScope"/>.</returns>
    ILogScope BeginScope(string name, LogLevel logLevel = LogLevel.Trace);

    /// <summary>
    /// Executes the specified action within this scope, capturing any exception.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    void Execute(Action action);

    /// <summary>
    /// Executes the specified asynchronous action within this scope, capturing any exception.
    /// </summary>
    /// <param name="action">The asynchronous action to execute.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Execute(Func<Task> action);

    /// <summary>
    /// Executes the specified asynchronous action within this scope, capturing any exception.
    /// </summary>
    /// <param name="action">The asynchronous action to execute.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(Func<Task> action);

    /// <summary>
    /// Executes the specified function within this scope, capturing any exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The function to execute.</param>
    /// <returns>The result of the function.</returns>
    TResult Execute<TResult>(Func<TResult> action);

    /// <summary>
    /// Executes the specified asynchronous function within this scope, capturing any exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The asynchronous function to execute.</param>
    /// <returns>A task representing the asynchronous operation with the result.</returns>
    Task<TResult> Execute<TResult>(Func<Task<TResult>> action);

    /// <summary>
    /// Executes the specified asynchronous function within this scope, capturing any exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The asynchronous function to execute.</param>
    /// <returns>A task representing the asynchronous operation with the result.</returns>
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
}
