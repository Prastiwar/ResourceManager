using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.DataSource;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using RpgDataEditor.DataSource;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlIntegrationTestClass : IntegrationTestClass
    {
        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService)
            => builder.AddSqlDataSource(o => {
                o.DescriptorService = descriptorService;
                o.CreateDatabaseContext = CreateSqlDbContext;
            });

        protected override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Sql");
                SqlProxyConfiguration proxy = new SqlProxyConfiguration(dataSourceConfiguration);
                string relativePath = "./Fixtures/Sql/Database.sqlite";
                string databaseFilePath = Path.GetFullPath(relativePath);
                proxy.ConnectionString = $"Data Source = {databaseFilePath}; Version = 3;";
                InitializeFixture("Sql");
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }

        protected virtual DbContext CreateSqlDbContext(string connectionString, IConfiguration configuration, SqlDataSourceOptions options)
            => new DefaultDbContext(connectionString, configuration);
    }
}
