using System;

namespace Yapoml.Framework.Options
{
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
