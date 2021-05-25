using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Sql.Configuration;

namespace ResourceManager.DataSource.Sql
{
    public class SqlDataSource : IDataSource
    {
        public SqlDataSource(IConfiguration configuration, IConnectionMonitor monitor, SqlDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public SqlDataSourceOptions Options { get; }
    }
}
