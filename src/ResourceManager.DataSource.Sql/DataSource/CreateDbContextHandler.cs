using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Sql.Configuration;

namespace ResourceManager.DataSource.Sql
{
    public delegate DbContext CreateDbContextHandler(string connectionString, IConfiguration configuration, SqlDataSourceOptions options);
}
