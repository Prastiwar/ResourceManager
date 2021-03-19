using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Core.Connection;
using System;
using System.IO;
using System.Threading.Tasks;
using RPGDataEditor.Core.Providers;

namespace RPGDataEditor.Core.Mvvm
{
    public class SessionContext : ObservableModel
    {
        public SessionContext() { }

        public SessionContext(string sessionFilePath)
        {
            if (string.IsNullOrEmpty(sessionFilePath))
            {
                throw new ArgumentException($"'{nameof(sessionFilePath)}' cannot be null or empty.", nameof(sessionFilePath));
            }
            SessionFilePath = sessionFilePath;
        }

        protected string SessionFilePath { get; set; }

        private OptionsData options = new OptionsData();
        public OptionsData Options {
            get => options;
            set => SetProperty(ref options, value ?? new OptionsData());
        }

        public Task ConnectAsync() => Task.CompletedTask;

        public Task DisconnectAsync() => Task.CompletedTask;

        private IConnectionService connectionService;
        public IConnectionService ConnectionService {
            get => connectionService;
            set => SetProperty(ref connectionService, value ?? throw new ArgumentNullException(nameof(ConnectionService)));
        }

        private IConnectionProvider connectionProvider;
        public IConnectionProvider ConnectionProvider {
            get => connectionProvider;
            set => SetProperty(ref connectionProvider, value ?? throw new ArgumentNullException(nameof(ConnectionService)));
        }

        public void SetConnection(string name)
        {
            IConnectionService newConnection = ConnectionProvider.GetConnectionService(name);
            if (newConnection.GetType() != ConnectionService.GetType())
            {
                ConnectionService = newConnection;
            }
        }

        public IJsonFilesController GetCurrentController() => ConnectionService as IJsonFilesController;

        public async void OnResourceChanged(RPGResource resource)
        {
            if (Options.BackupOnSaving)
            {
                await CreateBackupAsync(resource);
            }
        }

        public async Task<bool> CreateBackupAsync(RPGResource resource)
        {
            try
            {
                string relativePath = null;
                string backupPath = null;
                switch (resource)
                {
                    case RPGResource.Quest:
                        relativePath = "quests";
                        backupPath = Options.QuestsBackupPath;
                        break;
                    case RPGResource.Dialogue:
                        relativePath = "dialogues";
                        backupPath = Options.DialoguesBackupPath;
                        break;
                    case RPGResource.Npc:
                        relativePath = "npcs";
                        backupPath = Options.NpcBackupPath;
                        break;
                    default:
                        break;
                }
                if (relativePath != null)
                {
                    if (string.IsNullOrEmpty(backupPath))
                    {
                        return false;
                    }
                    string[] jsons = await GetCurrentController().GetJsonsAsync(relativePath);
                    string backupJson = JsonConvert.SerializeObject(jsons);
                    bool saved = await SaveJsonFileAsync(backupPath, backupJson);
                    return saved;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't make backup", ex);
            }
            return false;
        }

        public void SaveSession()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(SessionFilePath, json);
        }

        public SessionContext LoadSession()
        {
            string json = File.ReadAllText(SessionFilePath);
            SessionContext session = JsonConvert.DeserializeObject<SessionContext>(json);
            session.SessionFilePath = SessionFilePath;
            return session;
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
