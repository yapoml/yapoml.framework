using System;
using System.Collections.Generic;

namespace Yapoml.Options
{
    public class SpaceOptions : ISpaceOptions
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

    public class TypeRegisteredEventArgs : EventArgs
    {
        public TypeRegisteredEventArgs(Type type, object instance)
        {
            Type = type;
            Instance = instance;
        }

        public Type Type { get; }
        public object Instance { get; }
    }
}
