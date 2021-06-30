using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.Core.Services;
using ResourceManager.DataSource;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using System;
using System.Threading;

namespace RpgDataEditor.Tests.Local
{
    public class LocalIntegrationTestProvider : IntegrationTestProvider
    {
        public LocalIntegrationTestProvider() : base(Guid.NewGuid().ToString()) { }

        protected override string DataSourceName => "Local";

        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService)
            => builder.AddLocalDataSource(o => {
                o.DescriptorService = descriptorService;
                o.Serializer = new NewtonsoftSerializer();
            });

        public override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            DisposedCheck();
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Local");
                LocalProxyConfiguration proxy = new LocalProxyConfiguration(dataSourceConfiguration) {
                    FolderPath = $"./{FixtureFolderName}/{DataSourceName}-{TempKey}"
                };
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }
    }
}
