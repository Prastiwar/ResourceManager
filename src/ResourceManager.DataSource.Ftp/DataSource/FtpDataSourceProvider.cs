using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.DataSource.File;
using ResourceManager.DataSource.Ftp.Configuration;
using ResourceManager.DataSource.Ftp.Data;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSourceProvider : IDataSourceProvider
    {
        public FtpDataSourceProvider(FtpDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected FtpDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSource Provide(IServiceCollection services, IConfiguration configuration)
        {
            string host = configuration["host"];
            string userName = configuration["username"];
            System.Security.SecureString password = SecretString.DecryptString(configuration["password"]);
            int port = int.TryParse(configuration["port"], out int parsedPort) ? parsedPort : 0;
            FtpConnectionMonitor monitor = new FtpConnectionMonitor(host, new System.Net.NetworkCredential(userName, password), port);
            FtpDataSourceOptions options = new FtpDataSourceOptions() {
                Host = host,
                UserName = userName,
                Password = password,
                Port = port,
                RelativePath = configuration["relativepath"]
            };
            FtpDataSource dataSource = new FtpDataSource(configuration, monitor, options);
            FtpFileClient client = new FtpFileClient(options);
            services.AddSingleton<IFtpFileClient>(client);
            services.AddSingleton<IFileClient>(client);
            services.AddSingleton(dataSource);
            return dataSource;
        }
    }
}
