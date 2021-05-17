using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ResourceManager.DataSource.Ftp.Configuration;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSource : IDataSource
    {
        public FtpDataSource(IConfiguration configuration, IConnectionMonitor monitor, IOptions<FtpDataSourceOptions> options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public IOptions<FtpDataSourceOptions> Options { get; }
    }
}
