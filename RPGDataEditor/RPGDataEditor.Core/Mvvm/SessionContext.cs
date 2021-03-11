using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class SessionContext : ObservableModel
    {
        private string locationPath = "";
        public string LocationPath {
            get => locationPath;
            set {
                SetProperty(ref locationPath, value ?? "");
                ftpController.LocationPath = locationPath;
                explorerController.LocationPath = locationPath;
            }
        }

        private string ftpUserName = "";
        public string FtpUserName {
            get => ftpUserName;
            set {
                SetProperty(ref ftpUserName, value ?? "");
                ftpController.FtpUserName = ftpUserName;
            }
        }

        private string ftpPassword = "";
        public string FtpPassword {
            get => ftpPassword;
            set {
                SetProperty(ref ftpPassword, value ?? "");
                ftpController.FtpPassword = ftpPassword;
            }
        }

        private OptionsData options = new OptionsData();
        public OptionsData Options {
            get => options;
            set => SetProperty(ref options, value ?? new OptionsData());
        }

        private readonly FtpController ftpController = new FtpController();

        private readonly ExplorerController explorerController = new ExplorerController();

        public bool IsFtp => LocationPath.StartsWith("ftp:");

        public IJsonFilesController GetCurrentController() => IsFtp ? (IJsonFilesController)ftpController : explorerController;

        public void SaveSession(string path)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        public Task<bool> IsValidAsync() => GetCurrentController().IsValidAsync();

        public Task<bool> DeleteFileAsync(string relativePath) => GetCurrentController().DeleteFileAsync(relativePath);

        public Task<bool> SaveJsonFileAsync(string relativePath, string json) => GetCurrentController().SaveJsonAsync(relativePath, json);

        public Task<string[]> GetFilesAsync(string relativePath) => GetCurrentController().GetJsonFiles(relativePath);

        public Task<string> GetJsonAsync(string relativePath) => GetCurrentController().GetJsonAsync(relativePath);

        public Task<string[]> LoadJsonsAsync(string relativePath) => GetCurrentController().GetJsonsAsync(relativePath);

        public async Task<T[]> LoadAsync<T>(string relativePath)
        {
            string[] jsons = await LoadJsonsAsync(relativePath);
            T[] models = new T[jsons.Length];
            for (int i = 0; i < jsons.Length; i++)
            {
                models[i] = JsonConvert.DeserializeObject<T>(jsons[i]);
            }
            return models;
        }

    }
}
