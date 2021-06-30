using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.Core.Services;
using ResourceManager.DataSource;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using System.Threading;

namespace RpgDataEditor.Tests.Local
{
    public class LocalIntegrationTestClass : IntegrationTestClass
    {
        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService) 
            => builder.AddLocalDataSource(o => {
            o.DescriptorService = descriptorService;
            o.Serializer = new NewtonsoftSerializer();
        });

        protected override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Local");
                LocalProxyConfiguration proxy = new LocalProxyConfiguration(dataSourceConfiguration) {
                    FolderPath = "./Fixtures/Local-temp"
                };
                InitializeFixture("Local");
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }
    }
}
