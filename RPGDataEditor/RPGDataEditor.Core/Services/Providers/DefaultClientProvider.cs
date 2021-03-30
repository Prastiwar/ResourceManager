using RPGDataEditor.Core.Connection;
using System;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultClientProvider : IClientProvider
    {
        public DefaultClientProvider(IResourceToTypeConverter typeConverter, IResourceToPathConverter pathConverter)
        {
            this.typeConverter = typeConverter;
            this.pathConverter = pathConverter;
        }

        private readonly IResourceToTypeConverter typeConverter;
        private readonly IResourceToPathConverter pathConverter;

        public IResourceClient GetClient(string name)
        {
            bool isExplorer = string.Compare(name, "explorer", true) == 0;
            bool isFtp = string.Compare(name, "ftp", true) == 0;
            bool isMssql = string.Compare(name, "mssql", true) == 0;
            if (isExplorer)
            {
                return new ExplorerResourceClient(pathConverter, typeConverter);
            }
            if (isFtp)
            {
                return new FtpResourceClient(pathConverter, typeConverter);
            }
            if (isMssql)
            {
                //return new MssqlResourceClient();
            }
            throw new NotSupportedException($"{name} connection is not supported");
        }
    }
}