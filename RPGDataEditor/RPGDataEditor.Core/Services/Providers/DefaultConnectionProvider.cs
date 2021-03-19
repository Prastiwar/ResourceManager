using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Services;
using System;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultConnectionProvider : IConnectionProvider
    {
        public IConnectionService GetConnectionService(string name)
        {
            bool isFile = string.Compare(name, "file", true) == 0;
            bool isFtp = string.Compare(name, "ftp", true) == 0;
            bool isMssql = string.Compare(name, "mssql", true) == 0;
            if (isFile)
            {
                return new ExplorerController();
            }
            if (isFtp)
            {
                return new FtpController();
            }
            if (isMssql)
            {
            }
            throw new NotSupportedException($"{name} connection is not supported");
        }
    }
}