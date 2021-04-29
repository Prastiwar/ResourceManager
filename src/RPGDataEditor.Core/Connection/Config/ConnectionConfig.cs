using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Core.Connection
{
    public class ConnectionConfig : IConnectionConfig
    {
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public object Get([CallerMemberName] string parameter = null) => parameters.TryGetValue(parameter, out object value) ? value : null;

        public void Set(string parameter, object value) => parameters[parameter] = value;

        protected void Set(object value, [CallerMemberName] string parameter = null) => Set(parameter, value);
    }
}
