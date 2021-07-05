using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.DataSource;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using RpgDataEditor.DataSource;
using System;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlIntegrationTestProvider : IntegrationTestProvider
    {
        public SqlIntegrationTestProvider() : base(Guid.NewGuid().ToString()) { }

        protected override string DataSourceName => "Sql";

        protected override void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService)
            => builder.AddSqlDataSource(o => {
                o.DescriptorService = descriptorService;
                o.CreateDatabaseContext = CreateSqlDbContext;
            });

        public override IDataSource ConnectDataSource(CancellationToken token = default)
        {
            DisposedCheck();
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Sql");
                SqlProxyConfiguration proxy = new SqlProxyConfiguration(dataSourceConfiguration);
                string relativePath = $"{RootPath}/Database.sqlite";
                string databaseFilePath = Path.GetFullPath(relativePath);
                proxy.ConnectionString = $"Data Source={databaseFilePath}";
                proxy.EngineName = "sqlite";
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }

        protected virtual DbContext CreateSqlDbContext(string connectionString, IConfiguration configuration, SqlDataSourceOptions options)
            => new DefaultDbContext(connectionString, configuration);
    }
}
