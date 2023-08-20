using FluentAssertions;
using NUnit.Framework;
using Yapoml.Framework.Logging;

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
        }
    }
}
