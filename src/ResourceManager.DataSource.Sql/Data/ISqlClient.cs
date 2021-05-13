using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql.Data
{
    public interface ISqlClient
    {
        Task<IEnumerable<object>> SelectAsync(string table, Type type);

        Task<IEnumerable<object>> SelectAsync(string table, Type type, object[] ids);

        Task<object> SelectScalarAsync(string table, Type type, object id);

        IQueryable<object> Query(string table, Type type);

        string GetTableName(Type type);
    }
}
