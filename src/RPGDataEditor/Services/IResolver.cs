using System;

namespace RPGDataEditor.Services
{
    public interface IResolverService
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}
