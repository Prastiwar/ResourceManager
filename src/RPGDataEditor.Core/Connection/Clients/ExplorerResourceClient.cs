//using RPGDataEditor.Connection;
//using RPGDataEditor.Services;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace RPGDataEditor.Core.Connection
//{
//    public class LocalResourceClient : IContentResourceClient
//    {
//        public LocalResourceClient(ISerializer serializer) => Serializer = serializer;

//        public string FolderPath { get; set; }

//        public string FileSearchPattern { get; set; }

//        public IResourceDescriptorCollection Descriptors { get; set; }

//        protected ISerializer Serializer { get; }

//        public async Task<bool> UpdateAsync(object oldResource, object newResource)
//        {
//            bool saved = await SaveAsync(newResource, false);
//            if (saved)
//            {
//                await DeleteAsync(oldResource);
//            }
//            return saved;
//        }

//        public Task<bool> CreateAsync(object resource) => SaveAsync(resource, true);

//        public async Task<bool> CreateBackupAsync(ResourceDescriptor descriptor, string filePath)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filePath))
//                {
//                    return false;
//                }
//                object[] objects = await GetAllAsync(descriptor);
//                string backup = Serializer.Serialize(objects);
//                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
//                File.WriteAllText(filePath, backup);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't make backup", ex);
//            }
//            return false;
//        }

//        public Task<bool> DeleteAsync(object resource)
//        {
//            try
//            {
//                ResourceDescriptor descriptor = Descriptors.First(descriptor => descriptor.CanDescribe(resource)); ;
//                string filePath = Path.Combine(FolderPath, descriptor.GetRelativeFullPath(resource));
//                File.Delete(filePath);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't delete file", ex);
//                return Task.FromResult(false);
//            }
//            return Task.FromResult(true);
//        }

//        public async Task<object[]> GetAllAsync(ResourceDescriptor descriptor)
//        {
//            string[] contents = await GetAllContentAsync(descriptor);
//            return contents.Select(content => Serializer.Deserialize(content, descriptor.Type)).ToArray();
//        }

//        public async Task<string[]> GetAllContentAsync(ResourceDescriptor descriptor)
//        {
//            string directoryPath = Path.Combine(FolderPath, descriptor.RelativeRootPath);
//            List<string> contents = new List<string>();
//            if (Directory.Exists(directoryPath))
//            {
//                string[] files = await GetContentFiles(directoryPath);
//                foreach (string file in files)
//                {
//                    string content = File.ReadAllText(file);
//                    contents.Add(content);
//                }
//            }
//            return contents.ToArray();
//        }

//        public Task<string> GetContentAsync(string location)
//        {
//            string filePath = Path.Combine(FolderPath, location);
//            string content = File.ReadAllText(filePath);
//            return Task.FromResult(content);
//        }

//        public Task<string> GetContentAsync(object resource)
//        {
//            string filePath = Path.Combine(FolderPath, Descriptors.Describe(resource).GetRelativeFullPath(resource));
//            string content = File.ReadAllText(filePath);
//            return Task.FromResult(content);
//        }

//        public async Task<object> GetAsync(object resource)
//        {
//            string content = await GetContentAsync(resource);
//            return Serializer.Deserialize(content, Descriptors.Describe(resource).Type);
//        }

//        public async Task<object> GetAsync(Type type, string location)
//        {
//            string content = await GetContentAsync(location);
//            return Serializer.Deserialize(content, type);
//        }

//        public Task<string[]> GetAllLocationsAsync(ResourceDescriptor descriptor) => GetContentFiles(Path.Combine(FolderPath, descriptor.RelativeRootPath));

//        public Task<string> GetLocationAsync(object resource) => Task.FromResult(Descriptors.Describe(resource).GetRelativeFullPath(resource));

//        public Task<string[]> GetContentFiles(string directoryPath)
//            => Task.FromResult(EnumerateJsonFiles(directoryPath).ToArray());

//        public IEnumerable<string> EnumerateJsonFiles(string directoryPath)
//            => Directory.EnumerateFiles(directoryPath, FileSearchPattern, SearchOption.AllDirectories).Select(path => path.Replace("\\", "/"));

//        public Task<bool> SaveAsync(object resource, bool create)
//        {
//            string path = Path.Combine(FolderPath, Descriptors.Describe(resource).GetRelativeFullPath(resource));
//            if (create && File.Exists(path))
//            {
//                return Task.FromResult(false);
//            }
//            try
//            {
//                string content = Serializer.Serialize(resource);
//                Directory.CreateDirectory(Path.GetDirectoryName(path));
//                File.WriteAllText(path, content);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't save file", ex);
//                return Task.FromResult(false);
//            }
//            return Task.FromResult(true);
//        }

//        public Task<bool> ConnectAsync() => Task.FromResult(Directory.Exists(FolderPath));

//        public Task<bool> DisconnectAsync() => Task.FromResult(true);
//    }
//}
