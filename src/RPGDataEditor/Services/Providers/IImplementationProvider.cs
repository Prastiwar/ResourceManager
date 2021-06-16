using System;

namespace RPGDataEditor.Providers
{
    /// <summary> Provides implementation for abstract or interface type model </summary>
    public interface IImplementationProvider<T>
    {
        T Get();
        T Get(Type targetType);
    }
}
