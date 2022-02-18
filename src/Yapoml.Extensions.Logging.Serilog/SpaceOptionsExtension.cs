using Serilog;
using Yapoml.Extensions.Logging.Serilog;
using Yapoml.Options;

namespace Yapoml.Selenium
{
    public static class SpaceOptionsExtension
    {
        public static ISpaceOptions UseSerilog(this ISpaceOptions spaceOptions)
        {
            spaceOptions.Register<Yapoml.Logging.ILogger>(new SerilogAdapter(Log.Logger));

            return spaceOptions;
        }

        public static ISpaceOptions UseSerilog(this ISpaceOptions spaceOptions, ILogger logger)
        {
            spaceOptions.Register<Yapoml.Logging.ILogger>(new SerilogAdapter(logger));

            return spaceOptions;
        }
    }
}
