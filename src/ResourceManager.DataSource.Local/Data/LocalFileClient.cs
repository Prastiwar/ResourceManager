using ResourceManager.DataSource.File;
using ResourceManager.DataSource.Local.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local.Data
{
    public class LocalFileClient : IFileClient
    {
        public LocalFileClient(LocalDataSourceOptions options) => Options = options;

        protected LocalDataSourceOptions Options { get; }

        public Task<IEnumerable<string>> ListFilesAsync(string path)
            => Task.FromResult(Directory.EnumerateFiles(path, Options.FileSearchPattern, SearchOption.AllDirectories).Select(file => file.Replace("\\", "/")));

        public Task<string> ReadFileAsync(string file) => System.IO.File.ReadAllTextAsync(file);
    }
}
