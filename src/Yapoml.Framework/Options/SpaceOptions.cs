namespace Yapoml.Framework.Options
{
    public class SpaceOptions : ISpaceOptions
    {
        public IServicesContainer Services { get; } = new ServicesContainer();
    }
}
