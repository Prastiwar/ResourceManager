using Microsoft.Extensions.Options;
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
        public LocalFileClient(IOptions<LocalDataSourceOptions> options) => Options = options;

        protected IOptions<LocalDataSourceOptions> Options { get; }

        public Task<IEnumerable<string>> ListFilesAsync(string path) => Task.FromResult(Directory.EnumerateFiles(path, Options.Value.FileSearchPattern, SearchOption.AllDirectories).Select(file => file.Replace("\\", "/")));

        public Task<string> ReadFileAsync(string file) => System.IO.File.ReadAllTextAsync(file);
    }
}
