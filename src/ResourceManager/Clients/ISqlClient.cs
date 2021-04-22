using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceManager
{
    public interface ISqlClient
    {
        Task<IEnumerable<object>> SelectAsync(string table, Type type);

        Task<object> SelectScalarAsync(string table, Type type, object id);

        string GetTableName(Type type);
    }
}
