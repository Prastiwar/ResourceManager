using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class ExplorerResourceClient : ObservableModel, IContentResourceClient
    {
        public ExplorerResourceClient(IResourceToPathConverter pathConverter, IResourceToTypeConverter typeConverter)
        {
            this.pathConverter = pathConverter;
            this.typeConverter = typeConverter;
        }

        private string folderPath = "";
        public string FolderPath {
            get => folderPath;
            set => SetProperty(ref folderPath, value ?? "");
        }

        private readonly IResourceToPathConverter pathConverter;

        private readonly IResourceToTypeConverter typeConverter;

        public async Task<bool> UpdateAsync(IIdentifiable oldResource, IIdentifiable newResource)
        {
            bool saved = await SaveJsonAsync(newResource, false);
            if (saved)
            {
                await DeleteAsync(oldResource);
            }
            return saved;
        }

        public Task<bool> CreateAsync(IIdentifiable resource) => SaveJsonAsync(resource, true);

        public async Task<bool> CreateBackupAsync(int resource, string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return false;
                }
                IIdentifiable[] jsons = await GetAllAsync(resource);
                string backupJson = JsonConvert.SerializeObject(jsons);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, backupJson);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't make backup", ex);
            }
            return false;
        }

        public Task<bool> DeleteAsync(IIdentifiable resource)
        {
            try
            {
                string filePath = Path.Combine(FolderPath, pathConverter.ToRelativePath(resource));
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't delete json file", ex);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public async Task<IIdentifiable[]> GetAllAsync(int resource)
        {
            string[] jsons = await GetAllContentAsync(resource);
            return jsons.Select(json => JsonConvert.DeserializeObject(json, typeConverter.GetResourceType(resource)) as IIdentifiable).ToArray();
        }

        public async Task<string[]> GetAllContentAsync(int resource)
        {
            string directoryPath = Path.Combine(FolderPath, pathConverter.ToRelativeRoot(resource));
            List<string> jsons = new List<string>();
            if (Directory.Exists(directoryPath))
            {
                string[] files = await GetJsonFiles(directoryPath);
                foreach (string file in files)
                {
                    string json = File.ReadAllText(file);
                    jsons.Add(json);
                }
            }
            return jsons.ToArray();
        }

        public Task<string> GetContentAsync(IIdentifiable identifiable)
        {
            string filePath = Path.Combine(FolderPath, pathConverter.ToRelativePath(identifiable));
            string json = File.ReadAllText(filePath);
            return Task.FromResult(json);
        }

        public async Task<IIdentifiable> GetAsync(IIdentifiable identifiable)
        {
            string json = await GetContentAsync(identifiable);
            return JsonConvert.DeserializeObject(json, typeConverter.GetResourceType(identifiable)) as IIdentifiable;
        }
        public Task<string[]> GetAllLocationsAsync(int resource) => GetJsonFiles(Path.Combine(FolderPath, pathConverter.ToRelativeRoot(resource)));

        public Task<string> GetLocationAsync(IIdentifiable resource) => Task.FromResult(pathConverter.ToRelativePath(resource));

        public Task<string[]> GetJsonFiles(string directoryPath)
            => Task.FromResult(EnumerateJsonFiles(directoryPath).ToArray());

        public IEnumerable<string> EnumerateJsonFiles(string directoryPath)
            => Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.AllDirectories).Select(path => path.Replace("\\", "/"));

        public Task<bool> SaveJsonAsync(IIdentifiable resource, bool create)
        {
            string path = Path.Combine(FolderPath, pathConverter.ToRelativePath(resource));
            if (create && File.Exists(path))
            {
                return Task.FromResult(false);
            }
            try
            {
                string json = JsonConvert.SerializeObject(resource);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't save json file", ex);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool> ConnectAsync() => Task.FromResult(Directory.Exists(FolderPath));

        public Task<bool> DisconnectAsync() => Task.FromResult(true);
    }
}
