using System;

namespace ResourceManager.Data
{
    public interface ITextSerializer
    {
        string Serialize(object obj, Type type = null);

        object Deserialize(string value, Type type = null);
    }
}
