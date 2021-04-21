using System;

namespace RPGDataEditor.Services
{
    public interface ISerializer
    {
        string Serialize(object obj, Type type = null);

        object Deserialize(string value, Type type = null);
    }
}
