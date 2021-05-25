using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Local.Configuration;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSource : IDataSource
    {
        public LocalDataSource(IConfiguration configuration, IConnectionMonitor monitor, LocalDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public LocalDataSourceOptions Options { get; }
    }
}
