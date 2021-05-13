using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGDataEditor.Connection
{
    public class ConnectionConfig : IConnectionConfig
    {
        public ConnectionConfig(Dictionary<string, object> parameters) => this.parameters = parameters ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, object> parameters;

        public object Get(string parameter) => parameters[parameter];

        public object Get(string parameter, object defaultValue) => parameters.TryGetValue(parameter, out object value) ? value : defaultValue;

        public bool TryGet(string parameter, out object value) => parameters.TryGetValue(parameter, out value);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => parameters.GetEnumerator();
    }
}
