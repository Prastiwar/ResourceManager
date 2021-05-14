using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IDataSource
    {
        IConfiguration Configuration { get; }

        IConnectionMonitor Monitor { get; }
    }
}
