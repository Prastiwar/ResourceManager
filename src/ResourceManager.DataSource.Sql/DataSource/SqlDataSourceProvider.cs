using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.DataSource.Sql.Configuration;

namespace ResourceManager.DataSource.Sql
{
    public class SqlDataSourceProvider : IDataSourceProvider
    {
        public SqlDataSourceProvider(SqlDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected SqlDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSourceProviderBuilderOptions GetBuilderOptions() => BuilderOptions;

        public IDataSource Provide(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = string.IsNullOrEmpty(BuilderOptions.ConnectionString) ? BuilderOptions.ConnectionString : configuration["connectionstring"];
            SqlConnectionMonitor monitor = new SqlConnectionMonitor(connectionString);
            SqlDataSourceOptions options = new SqlDataSourceOptions() {
                ConnectionString = connectionString
            };
            SqlDataSource dataSource = new SqlDataSource(configuration, monitor, BuilderOptions.DescriptorService, options);
            services.AddSingleton(dataSource);
            return dataSource;
        }
    }
}
