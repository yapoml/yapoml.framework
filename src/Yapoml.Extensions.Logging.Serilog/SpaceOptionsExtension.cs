using Serilog;
using Yapoml.Options;

namespace Yapoml.Extensions.Logging.Serilog
{
    public static class SpaceOptionsExtension
    {
        public static ISpaceOptions UseSerilog(this ISpaceOptions spaceOptions)
        {
            spaceOptions.Logger = new SerilogAdapter(Log.Logger);

            return spaceOptions;
        }

        public static ISpaceOptions UseSerilog(this ISpaceOptions spaceOptions, ILogger logger)
        {
            spaceOptions.Logger = new SerilogAdapter(logger);

            return spaceOptions;
        }
    }
}
