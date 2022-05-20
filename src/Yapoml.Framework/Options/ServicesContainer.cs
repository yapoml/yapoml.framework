using System;
using System.Collections.Generic;

namespace Yapoml.Framework.Options
{
    internal class ServicesContainer : IServicesContainer
    {
        private IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        public event EventHandler<TypeRegisteredEventArgs> OnTypeRegistered;

        public void Register<T>(T instance)
        {
            _instances[typeof(T)] = instance;

            OnTypeRegistered?.Invoke(this, new TypeRegisteredEventArgs(typeof(T), instance));
        }

        public T Get<T>()
        {
            if (!_instances.TryGetValue(typeof(T), out var instance))
            {
                throw new Exception($"Cannot get an instance of {typeof(T)} type. Make sure the type is registered.");
            }

            return (T)instance;
        }
    }
}
