using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yapoml.Framework
{
    public static class Waiter
    {
        public static void Until(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
        {
            var stopwatch = Stopwatch.StartNew();

            Lazy<List<Exception>> occuredExceptions = new Lazy<List<Exception>>(() => new List<Exception>());

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
                    occuredExceptions.Value.Add(ex);

                    Thread.Sleep(pollingInterval);
                }
            }
            while (stopwatch.Elapsed <= timeout);

            var timeoutMessageBuilder = new StringBuilder($"Condition was not satisfied within {timeout.TotalSeconds} seconds when polled every {pollingInterval.TotalSeconds} seconds.");

            if (occuredExceptions.IsValueCreated)
            {
                timeoutMessageBuilder.AppendLine();
                timeoutMessageBuilder.AppendLine("Occured errors:");

                foreach (var occuredExceptionsGroup in occuredExceptions.Value.GroupBy(e => e.Message))
                {
                    timeoutMessageBuilder.AppendLine($" - {occuredExceptionsGroup.Key} ({occuredExceptionsGroup.Count()} times)");
                }
            }

            throw new TimeoutException(timeoutMessageBuilder.ToString());
        }

        public static async Task UntilAsync(Func<Task<bool>> condition, TimeSpan timeout, TimeSpan pollingInterval)
        {
            var stopwatch = Stopwatch.StartNew();

            Lazy<List<Exception>> occuredExceptions = new Lazy<List<Exception>>(() => new List<Exception>());

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
                    occuredExceptions.Value.Add(ex);

                    await Task.Delay(pollingInterval).ConfigureAwait(false);
                }
            }
            while (stopwatch.Elapsed <= timeout);

            var timeoutMessageBuilder = new StringBuilder($"Condition was not satisfied within {timeout.TotalSeconds} seconds when polled every {pollingInterval.TotalSeconds} seconds.");

            if (occuredExceptions.IsValueCreated)
            {
                timeoutMessageBuilder.AppendLine();
                timeoutMessageBuilder.AppendLine("Occured errors:");

                foreach (var occuredExceptionsGroup in occuredExceptions.Value.GroupBy(e => e.Message))
                {
                    timeoutMessageBuilder.AppendLine($" - {occuredExceptionsGroup.Key} ({occuredExceptionsGroup.Count()} times)");
                }
            }

            throw new TimeoutException(timeoutMessageBuilder.ToString());
        }
    }
}
