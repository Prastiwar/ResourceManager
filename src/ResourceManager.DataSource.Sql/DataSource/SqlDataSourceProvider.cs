using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ResourceManager.DataSource.Sql.Configuration;

namespace ResourceManager.DataSource.Sql
{
    public class SqlDataSourceProvider : IDataSourceProvider
    {
        public SqlDataSourceProvider(SqlDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected SqlDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSource Provide(IConfiguration configuration)
        {
            string connectionString = string.IsNullOrEmpty(BuilderOptions.ConnectionString) ? BuilderOptions.ConnectionString : configuration["connectionstring"];
            SqlConnectionMonitor monitor = new SqlConnectionMonitor(connectionString);
            IOptions<SqlDataSourceOptions> options = Options.Create(new SqlDataSourceOptions() {
                ConnectionString = connectionString
            });
            return new SqlDataSource(configuration, monitor, options);
        }
    }
}
