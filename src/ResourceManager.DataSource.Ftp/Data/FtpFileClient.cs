using FluentFTP;
using ResourceManager.DataSource.Ftp.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp.Data
{
    public class FtpFileClient : IFtpFileClient
    {
        public FtpFileClient(FtpDataSourceOptions options) => Options = options;

        protected FtpDataSourceOptions Options { get; }

        private FtpClient client;
        protected FtpClient Client {
            get {
                if (client == null)
                {
                    client = new FtpClient();
                }
                client.Host = Options.Host;
                client.Port = Options.Port;
                client.Credentials.UserName = Options.UserName;
                client.Credentials.SecurePassword = Options.Password;
                return client;
            }
        }

        public async Task<IEnumerable<string>> ListFilesAsync(string path)
        {
            string targetPath = Path.Combine(Options.RelativePath, path);
            FtpListItem[] items = await Client.GetListingAsync(targetPath, FtpListOption.Recursive);
            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
                        .Select(item => item.FullName).ToArray();
        }

        public async Task<string> ReadFileAsync(string file)
        {
            string targetPath = Path.Combine(Options.RelativePath, file);
            byte[] bytes = await Client.DownloadAsync(targetPath, default);
            if (bytes == null)
            {
                return null;
            }
            string content = Encoding.UTF8.GetString(bytes);
            return content;
        }
    }
}
