using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yapoml.Framework;

/// <summary>
/// Provides methods to repeatedly evaluate a condition until it succeeds or a timeout is reached.
/// </summary>
public static class Waiter
{
    /// <summary>
    /// Polls the specified condition at regular intervals until it returns <see langword="true"/> or the timeout expires.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="timeout">The maximum time to wait for the condition to be satisfied.</param>
    /// <param name="pollingInterval">The interval between condition evaluations.</param>
    /// <exception cref="TimeoutException">Thrown when the condition is not satisfied within the specified timeout.</exception>
    public static void Until(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
    {
        var stopwatch = Stopwatch.StartNew();

        Lazy<List<Exception>> occurredExceptions = new Lazy<List<Exception>>(() => new List<Exception>());

        do
        {
            try
            {
                var isSuccessful = condition();

                if (isSuccessful)
                {
                    return;
                }
                else
                {
                    Thread.Sleep(pollingInterval);
                }
            }
            catch (Exception ex)
            {
                occurredExceptions.Value.Add(ex);

                Thread.Sleep(pollingInterval);
            }
        }
        while (stopwatch.Elapsed <= timeout);

        var timeoutMessageBuilder = new StringBuilder($"Condition was not satisfied within {timeout.TotalSeconds} seconds when polled every {pollingInterval.TotalSeconds} seconds.");

        if (occurredExceptions.IsValueCreated)
        {
            timeoutMessageBuilder.AppendLine();
            timeoutMessageBuilder.AppendLine("Occurred errors:");

            foreach (var occurredExceptionsGroup in occurredExceptions.Value.GroupBy(e => e.Message))
            {
                timeoutMessageBuilder.AppendLine($" - {occurredExceptionsGroup.Key} ({occurredExceptionsGroup.Count()} times)");
            }
        }

        throw new TimeoutException(timeoutMessageBuilder.ToString());
    }

    /// <summary>
    /// Asynchronously polls the specified condition at regular intervals until it returns <see langword="true"/> or the timeout expires.
    /// </summary>
    /// <param name="condition">The asynchronous condition to evaluate.</param>
    /// <param name="timeout">The maximum time to wait for the condition to be satisfied.</param>
    /// <param name="pollingInterval">The interval between condition evaluations.</param>
    /// <returns>A task representing the asynchronous polling operation.</returns>
    /// <exception cref="TimeoutException">Thrown when the condition is not satisfied within the specified timeout.</exception>
    public static async Task UntilAsync(Func<Task<bool>> condition, TimeSpan timeout, TimeSpan pollingInterval)
    {
        var stopwatch = Stopwatch.StartNew();

        Lazy<List<Exception>> occurredExceptions = new Lazy<List<Exception>>(() => new List<Exception>());

        do
        {
            try
            {
                var isSuccessful = await condition().ConfigureAwait(false);

                if (isSuccessful)
                {
                    return;
                }
                else
                {
                    await Task.Delay(pollingInterval).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                occurredExceptions.Value.Add(ex);

                await Task.Delay(pollingInterval).ConfigureAwait(false);
            }
        }
        while (stopwatch.Elapsed <= timeout);

        var timeoutMessageBuilder = new StringBuilder($"Condition was not satisfied within {timeout.TotalSeconds} seconds when polled every {pollingInterval.TotalSeconds} seconds.");

        if (occurredExceptions.IsValueCreated)
        {
            timeoutMessageBuilder.AppendLine();
            timeoutMessageBuilder.AppendLine("Occurred errors:");

            foreach (var occurredExceptionsGroup in occurredExceptions.Value.GroupBy(e => e.Message))
            {
                timeoutMessageBuilder.AppendLine($" - {occurredExceptionsGroup.Key} ({occurredExceptionsGroup.Count()} times)");
            }
        }

        throw new TimeoutException(timeoutMessageBuilder.ToString());
    }
}
