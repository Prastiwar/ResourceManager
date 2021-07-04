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
            string connectionString = !string.IsNullOrEmpty(BuilderOptions.ConnectionString) ? BuilderOptions.ConnectionString : configuration["ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.ArgumentNullException("Connection string is needed");
            }
            SqlDataSourceOptions options = new SqlDataSourceOptions() {
                ConnectionString = connectionString
            };
            Microsoft.EntityFrameworkCore.DbContext context = BuilderOptions.CreateDatabaseContext.Invoke(connectionString, configuration, options);
            SqlConnectionMonitor monitor = new SqlConnectionMonitor(context);
            SqlDataSource dataSource = new SqlDataSource(configuration, monitor, BuilderOptions.DescriptorService, context, options);
            services.AddSingleton(dataSource);
            context.Database.EnsureCreated();
            return dataSource;
        }
    }
}
