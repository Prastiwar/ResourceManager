using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public interface IJsonFilesController
    {
        Task<bool> IsValidAsync();

        Task<bool> DeleteFileAsync(string relativePath);

        Task<bool> SaveJsonAsync(string relativePath, string json);

        Task<string> GetJsonAsync(string relativePath);

        Task<string[]> GetJsonsAsync(string relativePath);

        Task<string[]> GetJsonFiles(string relativePath);
    }
}
