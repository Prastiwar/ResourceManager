using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Connection
{
    public class ConnectionSettings : IConnectionSettings
    {
        public static class Connection
        {
            public const string SQL = "Sql";
            public const string LOCAL = "Local";
            public const string FTP = "Ftp";
        }

        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public IConnectionConfig CreateConfig() => new ConnectionConfig(parameters);

        public string Type {
            get => (string)Get();
            set => Set(value);
        }

        public object Get([CallerMemberName] string parameter = null) => parameters.TryGetValue(parameter, out object value) ? value : null;

        public void Set(string parameter, object value) => parameters[parameter] = value;

        public bool Clear(string parameter) => parameters.Remove(parameter);

        protected void Set(object value, [CallerMemberName] string parameter = null) => Set(parameter, value);

        public void Reset() => parameters.Clear();

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => parameters.GetEnumerator();
    }
}
