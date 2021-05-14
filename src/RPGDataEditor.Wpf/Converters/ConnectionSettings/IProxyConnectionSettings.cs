using Microsoft.Extensions.Configuration;

namespace RPGDataEditor.Wpf.Converters
{
    public interface IProxyConnectionSettings
    {
        IConfiguration Settings { get; }
    }
}
