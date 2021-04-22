using ResourceManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class SqlClient : ISqlClient
    {
        public string ConnectionString { get; set; }

        public string GetTableName(Type type) => throw new NotImplementedException();

        public Task<IEnumerable<object>> SelectAsync(string table, Type type) => throw new NotImplementedException();

        public Task<object> SelectScalarAsync(string table, Type type, object id) => throw new NotImplementedException();
    }
}
