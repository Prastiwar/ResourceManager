using System;

namespace RPGDataEditor.Core
{
    public interface ITypeToResourceConverter
    {
        int ToResource(Type type);
    }
}