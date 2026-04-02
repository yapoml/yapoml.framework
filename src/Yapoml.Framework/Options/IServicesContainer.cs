using System;

namespace Yapoml.Framework.Options;

/// <summary>
/// Provides a simple dependency injection container for registering and resolving service instances.
/// </summary>
public interface IServicesContainer
{
    /// <summary>
    /// Registers the specified instance as the implementation of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to register the instance as.</typeparam>
    /// <param name="instance">The service instance to register.</param>
    void Register<T>(T instance);

    /// <summary>
    /// Occurs when a new type is registered in the container.
    /// </summary>
    event EventHandler<TypeRegisteredEventArgs> OnTypeRegistered;

    /// <summary>
    /// Gets the registered instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>The registered service instance.</returns>
    /// <exception cref="ServiceNotFoundException">Thrown when no instance of <typeparamref name="T"/> is registered.</exception>
    T Get<T>();

    /// <summary>
    /// Attempts to get the registered instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <param name="service">When this method returns, contains the service instance if found; otherwise, the default value for the type.</param>
    /// <returns><see langword="true"/> if a service of the specified type was found; otherwise, <see langword="false"/>.</returns>
    bool TryGet<T>(out T service);
}
