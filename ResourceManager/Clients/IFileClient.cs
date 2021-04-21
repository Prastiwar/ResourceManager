using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceManager
{
    public interface IFileClient
    {
        Task<IEnumerable<string>> ListFilesAsync(string path);

        Task<string> ReadFileAsync(string file);
    }
}
