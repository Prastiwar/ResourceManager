using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.Core.Services;
using ResourceManager.DataSource;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests.Ftp
{
    public class FtpIntegrationTestClass : IntegrationTestClass
    {
        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService) 
            => builder.AddFtpDataSource(o => {
            o.DescriptorService = descriptorService;
            o.Serializer = new NewtonsoftSerializer();
        });

        protected override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Ftp");
                FtpProxyConfiguration proxy = new FtpProxyConfiguration(dataSourceConfiguration) {
                    Host = "localhost",
                    UserName = "testerro",
                    Password = "123456".ToSecure()
                };
                string relativePath = "Fixtures/Ftp-temp";
                string rootPath = Path.GetFullPath(relativePath);
                InitializeFixture("Ftp");
                FtpTestServer server = new FtpTestServer(rootPath);
                server.Start();
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }
    }
}
