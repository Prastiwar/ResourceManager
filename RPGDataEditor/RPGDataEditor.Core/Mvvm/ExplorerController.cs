using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class ExplorerController : ObservableModel, IJsonFilesController
    {
        private string locationPath = "";
        public string LocationPath {
            get => locationPath;
            set => SetProperty(ref locationPath, value ?? "");
        }

        public Task<bool> IsValidAsync() => Task.FromResult(Directory.Exists(LocationPath));

        public Task<bool> DeleteFileAsync(string relativePath)
        {
            try
            {
                string filePath = Path.Combine(LocationPath, relativePath);
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
                string path = Path.Combine(LocationPath, relativePath);
                new FileInfo(path).Directory.Create();
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't save json file", ex);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<string> GetJsonAsync(string relativePath) => Task.FromResult(File.ReadAllText(Path.Combine(LocationPath, relativePath)));

        public async Task<string[]> GetJsonsAsync(string relativePath)
        {
            List<string> jsons = new List<string>();
            string directoryPath = Path.Combine(LocationPath, relativePath);
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
            string directoryPath = Path.Combine(LocationPath, relativePath);
            return Task.FromResult(Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.AllDirectories).ToArray());
        }

    }
}
