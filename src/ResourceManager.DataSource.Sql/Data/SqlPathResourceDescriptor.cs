using ResourceManager.Data;
using System;

namespace ResourceManager.DataSource.Sql.Data
{
    public class SqlPathResourceDescriptor : PathResourceDescriptor
    {
        public SqlPathResourceDescriptor(Type type, string tableName, string pathFormat) : base(type, tableName, pathFormat) { }

        public string TableName => RelativeRootPath;
    }
}
