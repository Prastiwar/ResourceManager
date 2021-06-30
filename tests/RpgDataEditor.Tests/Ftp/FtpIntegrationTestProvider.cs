using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.Core.Services;
using ResourceManager.DataSource;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using System;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests.Ftp
{
    public class FtpIntegrationTestProvider : IntegrationTestProvider
    {
        public FtpIntegrationTestProvider() : base(Guid.NewGuid().ToString()) { }

        protected override string DataSourceName => "Ftp";

        protected override string RootPath => $"./{FixtureFolderName}/{DataSourceName}-temp/{TempKey}";

        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService)
            => builder.AddFtpDataSource(o => {
                o.DescriptorService = descriptorService;
                o.Serializer = new NewtonsoftSerializer();
            });

        public override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            DisposedCheck();
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Ftp");
                FtpProxyConfiguration proxy = new FtpProxyConfiguration(dataSourceConfiguration) {
                    Host = "localhost",
                    UserName = "testerro",
                    Password = "123456".ToSecure(),
                    RelativePath = TempKey
                };
                string relativePath = $"{FixtureFolderName}/{DataSourceName}-temp";
                string rootPath = Path.GetFullPath(relativePath);
                FtpTestServer server = new FtpTestServer(rootPath);
                server.Start();
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }
    }
}
