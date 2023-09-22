using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Logging.Sinks;

namespace Yapoml.Framework.Test.Logging
{
    internal class LoggingFixture
    {
        [Test]
        public void ShouldNotifyWhenLogTraceMessage()
        {
            var logger = new Logger();

            LogMessageEventArgs actualArgs = null;

            logger.OnLogMessage += (sender, args) => actualArgs = args;

            logger.Trace("test");

            actualArgs.Message.Should().Be("test");
            actualArgs.LogLevel.Should().Be(LogLevel.Trace);
            actualArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));

            actualArgs.LogScope.Should().BeNull();
        }

        [Test]
        public void ShouldNotifyWhenLogTraceMessageInLogScope()
        {
            var logger = new Logger();

            LogMessageEventArgs actualArgs = null;

            logger.OnLogMessage += (sender, args) => actualArgs = args;

            ILogScope logScope;

            using (logScope = logger.BeginLogScope("scope1"))
            {
                logger.Trace("test");
            }

            actualArgs.Message.Should().Be("test");
            actualArgs.LogLevel.Should().Be(LogLevel.Trace);
            actualArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));

            actualArgs.LogScope.Should().Be(logScope);
        }

        [Test]
        public void ShouldRaiseOnLogScopeBegin()
        {
            var logger = new Logger();

            LogScopeEventArgs actualArgs = null;

            logger.OnLogScopeBegin += (sender, args) => actualArgs = args;

            logger.BeginLogScope("test");

            actualArgs.Should().NotBeNull();
            actualArgs.LogScope.Name.Should().Be("test");
            actualArgs.LogScope.Depth.Should().Be(0);
            actualArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void ShouldRaiseOnLogScopeEnd()
        {
            var logger = new Logger();

            LogScopeEventArgs actualArgs = null;

            logger.OnLogScopeEnd += (sender, args) => actualArgs = args;

            var logScope = logger.BeginLogScope("test");
            logScope.Dispose();

            actualArgs.Should().NotBeNull();
            actualArgs.LogScope.Name.Should().Be("test");
            actualArgs.LogScope.Depth.Should().Be(0);
            actualArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void LogScopeShouldHaveDepth()
        {
            var logger = new Logger();

            ILogScope logScope1;
            ILogScope logScope2;
            ILogScope logScope3;

            using (logScope1 = logger.BeginLogScope("scope1"))
            {
                logScope1.Depth.Should().Be(0);

                using (logScope2 = logScope1.BeginScope("scope2"))
                {
                    logScope2.Depth.Should().Be(1);

                    using (logScope3 = logScope2.BeginScope("scope3"))
                    {
                        logScope3.Depth.Should().Be(2);
                    }

                    using (logScope3 = logScope2.BeginScope("scope3"))
                    {
                        logScope3.Depth.Should().Be(2);
                    }
                }
            }
        }
    }
}
