namespace Yapoml.Framework.Options;

/// <inheritdoc/>
public class SpaceOptions : ISpaceOptions
{
    /// <inheritdoc/>
    public IServicesContainer Services { get; } = new ServicesContainer();
}
