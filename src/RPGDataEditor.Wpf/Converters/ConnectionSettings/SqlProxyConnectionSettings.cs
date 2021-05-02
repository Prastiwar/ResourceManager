using RPGDataEditor.Connection;

namespace RPGDataEditor.Wpf.Converters
{
    public class SqlProxyConnectionSettings : ProxyConnectionSettings
    {
        public SqlProxyConnectionSettings(IConnectionSettings settings) : base(settings) { }

        public string ConnectionString {
            get => (string)Get();
            set => Set(value);
        }
    }
}
