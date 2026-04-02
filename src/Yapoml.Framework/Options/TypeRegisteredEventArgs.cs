using System;

namespace Yapoml.Framework.Options;

/// <summary>
/// Provides data for the <see cref="IServicesContainer.OnTypeRegistered"/> event.
/// </summary>
public class TypeRegisteredEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegisteredEventArgs"/> class.
    /// </summary>
    /// <param name="type">The type that was registered.</param>
    /// <param name="instance">The instance that was registered.</param>
    public TypeRegisteredEventArgs(Type type, object instance)
    {
        Type = type;
        Instance = instance;
    }

    /// <summary>
    /// Gets the type that was registered.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the instance that was registered.
    /// </summary>
    public object Instance { get; }
}
