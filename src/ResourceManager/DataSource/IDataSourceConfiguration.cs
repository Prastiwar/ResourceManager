using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IDataSourceConfiguration
    {
        string Type { get; }

        IConfiguration Configuration { get; }
    }
}
