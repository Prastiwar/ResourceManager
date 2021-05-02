using RPGDataEditor.Connection;

namespace RPGDataEditor.Wpf.Converters
{
    public class LocalProxyConnectionSettings : ProxyConnectionSettings
    {
        public LocalProxyConnectionSettings(IConnectionSettings settings) : base(settings) { }

        public string FolderPath {
            get => (string)Get();
            set => Set(value);
        }

        public string FileSearchPattern {
            get => (string)Get();
            set => Set(value);
        }
    }
}
