using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Ftp.Configuration;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSource : IDataSource
    {
        public FtpDataSource(IConfiguration configuration, IConnectionMonitor monitor, FtpDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public FtpDataSourceOptions Options { get; }
    }
}
