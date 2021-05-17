using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ResourceManager.DataSource.Ftp.Configuration;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSourceProvider : IDataSourceProvider
    {
        public FtpDataSourceProvider(FtpDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected FtpDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSource Provide(IConfiguration configuration)
        {
            string host = configuration["host"];
            string userName = configuration["username"];
            string password = configuration["password"];
            int port = int.TryParse(configuration["port"], out int parsedPort) ? parsedPort : 0;
            FtpConnectionMonitor monitor = new FtpConnectionMonitor(host, new System.Net.NetworkCredential(userName, password), port);
            IOptions<FtpDataSourceOptions> options = Options.Create(new FtpDataSourceOptions() {
                Host = host,
                UserName = userName,
                Password = password.ToSecure(),
                Port = port,
                RelativePath = configuration["relativepath"]
            });
            return new FtpDataSource(configuration, monitor, options);
        }
    }
}
