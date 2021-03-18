using System.Threading.Tasks;

namespace RPGDataEditor.Core.Providers
{
    public interface IResourceProvider
    {
        Task<IIdentifiable[]> LoadResourcesAsync(int resource);
    }

}