using Microsoft.Extensions.Options;
using ResourceManager.DataSource.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql.Data
{
    public class SqlClient : ISqlClient
    {
        public SqlClient(IOptions<SqlDataSourceOptions> options) => Options = options;

        public IOptions<SqlDataSourceOptions> Options { get; }

        public string GetTableName(Type type) => throw new NotImplementedException();
        public IQueryable<object> Query(string table, Type type) => throw new NotImplementedException();
        public Task<IEnumerable<object>> SelectAsync(string table, Type type) => throw new NotImplementedException();
        public Task<IEnumerable<object>> SelectAsync(string table, Type type, object[] ids) => throw new NotImplementedException();
        public Task<object> SelectScalarAsync(string table, Type type, object id) => throw new NotImplementedException();
    }
}
