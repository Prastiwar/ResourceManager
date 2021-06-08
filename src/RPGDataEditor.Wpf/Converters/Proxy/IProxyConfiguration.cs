using Microsoft.Extensions.Configuration;

namespace RPGDataEditor.Wpf.Converters
{
    public interface IProxyConfiguration
    {
        IConfiguration Configuration { get; }
    }
}
