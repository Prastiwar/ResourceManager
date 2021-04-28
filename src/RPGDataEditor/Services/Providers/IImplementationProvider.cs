using System;

namespace RPGDataEditor.Providers
{
    public interface IImplementationProvider<T>
    {
        T Get();
        T Get(Type targetType);
    }
}
