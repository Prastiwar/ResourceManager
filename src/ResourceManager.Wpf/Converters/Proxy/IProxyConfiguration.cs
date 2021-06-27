using Microsoft.Extensions.Configuration;

namespace ResourceManager.Wpf.Converters
{
    public interface IProxyConfiguration
    {
        IConfiguration Configuration { get; }
    }
}
