using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File
{
    public interface IFileClient
    {
        Task<IEnumerable<string>> ListFilesAsync(string path);

        Task<string> ReadFileAsync(string file);
    }
}
