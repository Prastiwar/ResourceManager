using System.Collections;
using System.Collections.Generic;

namespace RPGDataEditor.Connection
{
    public class ConnectionConfig : IConnectionConfig
    {
        public ConnectionConfig(Dictionary<string, object> parameters) => this.parameters = parameters ?? new Dictionary<string, object>();

        private readonly Dictionary<string, object> parameters;

        public object Get(string parameter) => parameters[parameter];

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => parameters.GetEnumerator();
    }
}
