using FluentFTP;
using ResourceManager;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class FtpFileClient : IFileClient
    {
        public string Host { get; set; }

        public string RelativePath { get; set; }

        public string UserName { get; set; }

        public SecureString Password { get; set; }

        public int Port { get; set; }

        protected FtpClient Client { get; set; }

        public async Task<IEnumerable<string>> ListFilesAsync(string path)
        {

            string targetPath = Path.Combine(RelativePath, path);
            FtpListItem[] items = await Client.GetListingAsync(targetPath, FtpListOption.Recursive);
            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
                        .Select(item => item.FullName).ToArray();
        }

        public async Task<string> ReadFileAsync(string file)
        {
            string targetPath = Path.Combine(RelativePath, file);
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
