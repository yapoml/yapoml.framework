using System;

namespace Yapoml.Framework.Options;

/// <summary>
/// The exception that is thrown when a requested service type is not registered in the container.
/// </summary>
public class ServiceNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class for the specified service type.
    /// </summary>
    /// <param name="serviceType">The type of the service that was not found.</param>
    public ServiceNotFoundException(Type serviceType)
        : base($"Cannot get an instance of {serviceType} type. Make sure the type is registered.")
    {
        ServiceType = serviceType;
    }

    /// <summary>
    /// Gets the type of the service that was not found.
    /// </summary>
    public Type ServiceType { get; }
}
