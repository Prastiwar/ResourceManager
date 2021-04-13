using RPGDataEditor.Core.Connection;
using System.Net;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultConnectionCheckerProvider : IConnectionCheckerProvider
    {
        public IConnectionChecker GetConnectionChecker(IResourceClient client)
        {
            if (client is FtpResourceClient ftpClient)
            {
                return new FtpConnectionChecker(ftpClient.Host, new NetworkCredential(ftpClient.UserName, ftpClient.Password), ftpClient.Port);
            }
            else if (client is MssqlResourceClient mssqlClient)
            {
                return new SqlConnectionChecker(mssqlClient.ConnectionString);
            }
            return null;
        }
    }
}
