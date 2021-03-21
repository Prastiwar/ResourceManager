using RPGDataEditor.Core.Models;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class MssqlResourceClient : ObservableModel, IResourceClient
    {
        private string connectionString = "";
        public string ConnectionString {
            get => connectionString;
            set => SetProperty(ref connectionString, value ?? "");
        }

        public Task<bool> ConnectAsync() => throw new NotImplementedException();
        public Task<bool> CreateAsync(IIdentifiable resource) => throw new NotImplementedException();
        public Task<bool> CreateBackupAsync(int resource, string filePath) => throw new NotImplementedException();
        public Task<bool> DeleteAsync(IIdentifiable resource) => throw new NotImplementedException();
        public Task<bool> DisconnectAsync() => throw new NotImplementedException();
        public Task<IIdentifiable[]> GetAllAsync(int resource) => throw new NotImplementedException();
        public Task<string[]> GetAllLocationsAsync(int resource) => throw new NotImplementedException();
        public Task<IIdentifiable> GetAsync(IIdentifiable resource) => throw new NotImplementedException();
        public Task<string> GetLocationAsync(IIdentifiable resource) => throw new NotImplementedException();
        public Task<bool> UpdateAsync(IIdentifiable oldResource, IIdentifiable newResource) => throw new NotImplementedException();
    }
}
