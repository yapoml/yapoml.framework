namespace Yapoml.Framework.Options;

/// <summary>
/// Provides access to the configuration options for a Yapoml space.
/// </summary>
public interface ISpaceOptions
{
    /// <summary>
    /// Gets the services container used to register and resolve dependencies.
    /// </summary>
    IServicesContainer Services { get; }
}