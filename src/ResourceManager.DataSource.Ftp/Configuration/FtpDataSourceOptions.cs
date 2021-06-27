using System.Security;

namespace ResourceManager.DataSource.Ftp.Configuration
{
    public class FtpDataSourceOptions
    {
        public string Host { get; set; }

        public string RelativePath { get; set; }

        public string UserName { get; set; }

        public SecureString Password { get; set; }

        public int Port { get; set; }
    }
}
