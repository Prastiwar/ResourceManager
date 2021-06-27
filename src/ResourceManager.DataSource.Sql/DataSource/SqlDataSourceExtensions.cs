using ResourceManager.DataSource.Sql;
using System;

namespace ResourceManager.DataSource
{
    public static class SqlDataSourceExtensions
    {
        public const string Name = "Sql";

        public static IConfigurableDataSourceBuilder AddSqlDataSource(this IConfigurableDataSourceBuilder builder, Action<SqlDataSourceBuilderOptions> options = null)
        {
            SqlDataSourceBuilderOptions builderOptions = new SqlDataSourceBuilderOptions {
                Name = Name,
            };
            options?.Invoke(builderOptions);
            if (builderOptions.CreateDatabaseContext is null)
            {
                throw new ArgumentNullException(nameof(builderOptions.CreateDatabaseContext), $"Yout must provide {typeof(CreateDbContextHandler)} to use this provider");
            }
            SqlDataSourceProvider provider = new SqlDataSourceProvider(builderOptions);
            return builder.Add(builderOptions.Name, provider);
        }
    }
}
