using Microsoft.Extensions.Configuration;

namespace ResourceManager.Wpf.Converters
{
    public class LocalProxyConfiguration : ProxyConfiguration
    {
        public LocalProxyConfiguration(IConfiguration settings) : base(settings) { }

        public string FolderPath {
            get => Get();
            set => Set(value);
        }

        public string FileSearchPattern {
            get => Get();
            set => Set(value);
        }
    }
}
