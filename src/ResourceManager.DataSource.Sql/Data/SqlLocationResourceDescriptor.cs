using ResourceManager.Data;
using System;

namespace ResourceManager.DataSource.Sql.Data
{
    public class SqlLocationResourceDescriptor : LocationResourceDescriptor
    {
        public SqlLocationResourceDescriptor(Type type, string tableName, string pathFormat) : base(type, tableName, pathFormat) { }

        public string TableName => RelativeRootPath;
    }
}
