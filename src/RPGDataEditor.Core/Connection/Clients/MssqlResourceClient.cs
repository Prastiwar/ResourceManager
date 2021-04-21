//using RPGDataEditor.Connection;
//using RPGDataEditor.Services;
//using System;
//using System.Threading.Tasks;

//namespace RPGDataEditor.Core.Connection
//{
//    public class MssqlResourceClient : IResourceClient
//    {
//        public string ConnectionString { get; set; }

//        public IResourceDescriptorCollection Descriptors { get; set; }

//        public Task<bool> ConnectAsync() => throw new NotImplementedException();
//        public Task<bool> CreateAsync(object resource) => throw new NotImplementedException();
//        public Task<bool> CreateBackupAsync(ResourceDescriptor descriptor, string filePath) => throw new NotImplementedException();
//        public Task<bool> DeleteAsync(object resource) => throw new NotImplementedException();
//        public Task<bool> DisconnectAsync() => throw new NotImplementedException();
//        public Task<object[]> GetAllAsync(ResourceDescriptor descriptor) => throw new NotImplementedException();
//        public Task<string[]> GetAllLocationsAsync(ResourceDescriptor descriptor) => throw new NotImplementedException();
//        public Task<object> GetAsync(object resource) => throw new NotImplementedException();
//        public Task<object> GetAsync(Type type, string location) => throw new NotImplementedException();
//        public Task<string> GetLocationAsync(object resource) => throw new NotImplementedException();
//        public Task<bool> UpdateAsync(object oldResource, object newResource) => throw new NotImplementedException();
//    }
//}
