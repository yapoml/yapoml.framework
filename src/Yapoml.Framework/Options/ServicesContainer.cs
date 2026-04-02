using System;
using System.Collections.Generic;

namespace Yapoml.Framework.Options;

internal class ServicesContainer : IServicesContainer
{
    private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

    public event EventHandler<TypeRegisteredEventArgs>? OnTypeRegistered;

    public void Register<T>(T instance)
    {
        if (instance is null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        _instances[typeof(T)] = instance;

        OnTypeRegistered?.Invoke(this, new TypeRegisteredEventArgs(typeof(T), instance));
    }

    public T Get<T>()
    {
        if (!_instances.TryGetValue(typeof(T), out var instance))
        {
            throw new ServiceNotFoundException(typeof(T));
        }

        return (T)instance;
    }

    public bool TryGet<T>(out T service)
    {
        var result = _instances.TryGetValue(typeof(T), out object? output);

        service = result ? (T)output! : default!;

        return result;
    }
}
