using System.Collections.Generic;

namespace RPGDataEditor.Connection
{
    public interface IConnectionConfig : IEnumerable<KeyValuePair<string, object>>
    {
        object Get(string parameter);

        object Get(string parameter, object defaultValue);

        bool TryGet(string parameter, out object value);
    }
}
