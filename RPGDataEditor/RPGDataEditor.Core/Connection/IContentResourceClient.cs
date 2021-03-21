using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public interface IContentResourceClient : IResourceClient
    {
        Task<string[]> GetAllContentAsync(int resource);

        Task<string> GetContentAsync(IIdentifiable resource);
    }
}
