using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public interface IResourceClient
    {
        Task<string[]> GetAllLocationsAsync(int resource);

        Task<string> GetLocationAsync(IIdentifiable resource);

        Task<IIdentifiable[]> GetAllAsync(int resource);

        Task<IIdentifiable> GetAsync(IIdentifiable resource);

        Task<IIdentifiable> GetAsync(Type type, string location);

        Task<bool> CreateAsync(IIdentifiable resource);

        Task<bool> UpdateAsync(IIdentifiable oldResource, IIdentifiable newResource);

        Task<bool> DeleteAsync(IIdentifiable resource);

        Task<bool> CreateBackupAsync(int resource, string filePath);

        Task<bool> ConnectAsync();

        Task<bool> DisconnectAsync();
    }
}
