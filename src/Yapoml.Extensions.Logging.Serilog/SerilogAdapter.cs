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

        public void Trace(string message)
        {
            _logger.Verbose(message);
        }
    }
}
