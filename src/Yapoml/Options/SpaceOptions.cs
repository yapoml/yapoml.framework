using Yapoml.Logging;

namespace Yapoml.Options
{
    public class SpaceOptions : ISpaceOptions
    {
        public ILogger Logger { get; set; } = new ConsoleLogger();
    }
}
