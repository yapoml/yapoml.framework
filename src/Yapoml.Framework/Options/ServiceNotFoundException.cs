using System;

namespace Yapoml.Framework.Options;

/// <summary>
/// The exception that is thrown when a requested service type is not registered in the container.
/// </summary>
public class ServiceNotFoundException : InvalidOperationException
{
    public ServiceNotFoundException(Type serviceType)
        : base($"Cannot get an instance of {serviceType} type. Make sure the type is registered.")
    {
        ServiceType = serviceType;
    }

    public Type ServiceType { get; }
}
