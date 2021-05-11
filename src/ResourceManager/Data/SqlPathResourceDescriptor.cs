using System;

namespace ResourceManager.Data
{
    public class SqlPathResourceDescriptor : PathResourceDescriptor
    {
        public SqlPathResourceDescriptor(Type type, string tableName, string pathFormat) : base(type, tableName, pathFormat) { }

        public string TableName => RelativeRootPath;
    }
}
