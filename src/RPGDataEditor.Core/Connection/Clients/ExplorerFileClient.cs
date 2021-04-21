using ResourceManager;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class ExplorerFileClient : IFileClient
    {
        public string FolderPath { get; set; }

        public string FileSearchPattern { get; set; }

        public Task<IEnumerable<string>> ListFilesAsync(string path) => Task.FromResult(Directory.EnumerateFiles(path, FileSearchPattern, SearchOption.AllDirectories).Select(file => file.Replace("\\", "/")));

        public Task<string> ReadFileAsync(string file) => Task.FromResult(File.ReadAllText(file));
    }
}
