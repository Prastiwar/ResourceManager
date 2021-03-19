using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class ExplorerController : ObservableModel, IJsonFilesController, IConnectionService
    {
        private string folderPath = "";
        public string FolderPath {
            get => folderPath;
            set => SetProperty(ref folderPath, value ?? "");
        }

        public Task<bool> IsValidAsync() => Task.FromResult(Directory.Exists(FolderPath));

        public Task<bool> DeleteFileAsync(string relativePath)
        {
            try
            {
                string filePath = Path.Combine(FolderPath, relativePath);
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't delete json file", ex);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool> SaveJsonAsync(string relativePath, string json)
        {
            try
            {
                string path = Path.Combine(FolderPath, relativePath);
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

        public Task<string> GetJsonAsync(string relativePath) => Task.FromResult(File.ReadAllText(Path.Combine(FolderPath, relativePath)));

        public async Task<string[]> GetJsonsAsync(string relativePath)
        {
            List<string> jsons = new List<string>();
            string directoryPath = Path.Combine(FolderPath, relativePath);
            if (Directory.Exists(directoryPath))
            {
                string[] files = await GetJsonFiles(relativePath);
                foreach (string file in files)
                {
                    jsons.Add(File.ReadAllText(file));
                }
            }
            return jsons.ToArray();
        }

        public Task<string[]> GetJsonFiles(string relativePath)
        {
            string directoryPath = Path.Combine(FolderPath, relativePath);
            return Task.FromResult(Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.AllDirectories).Select(path => path.Replace("\\", "/")).ToArray());
        }

        public Task<bool> ConnectAsync() => Task.FromResult(Directory.Exists(FolderPath));
        
        public Task<bool> DisconnectAsync() => Task.FromResult(true);
    }
}
