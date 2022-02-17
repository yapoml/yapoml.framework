using Serilog;

namespace Yapoml.Extensions.Logging.Serilog
{
    public class SerilogAdapter : Yapoml.Logging.ILogger
    {
        private readonly ILogger _logger;

        public SerilogAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(Yapoml.Logging.LogLevel level, string message)
        {
            _logger.Debug(message);
        }
    }
}
