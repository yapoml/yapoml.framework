using System;

namespace Yapoml.Framework.Options
{
    public interface ISpaceOptions
    {
        void Register<T>(T instance);

        event EventHandler<TypeRegisteredEventArgs> OnTypeRegistered;

        T Get<T>();
    }
}