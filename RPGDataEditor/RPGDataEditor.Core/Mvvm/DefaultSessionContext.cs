using Newtonsoft.Json;
using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class DefaultSessionContext : ObservableModel, ISessionContext
    {
        public DefaultSessionContext() { }

        public DefaultSessionContext(string sessionFilePath)
        {
            if (string.IsNullOrEmpty(sessionFilePath))
            {
                throw new ArgumentException($"'{nameof(sessionFilePath)}' cannot be null or empty.", nameof(sessionFilePath));
            }
            SessionFilePath = sessionFilePath;
        }

        public string SessionFilePath { get; set; }

        private OptionsData options;
        public OptionsData Options {
            get => options;
            set => SetProperty(ref options, value);
        }

        private IResourceClient client;
        public IResourceClient Client {
            get => client;
            set => SetProperty(ref client, value ?? throw new ArgumentNullException(nameof(Client)));
        }

        private IClientProvider clientProvider;
        public IClientProvider ClientProvider {
            get => clientProvider;
            set => SetProperty(ref clientProvider, value ?? throw new ArgumentNullException(nameof(ClientProvider)));
        }

        public void SetConnection(string name)
        {
            IResourceClient newClient = ClientProvider.GetClient(name);
            if (Client == null || newClient.GetType() != Client.GetType())
            {
                Client = newClient;
            }
        }

        public async void OnResourceChanged(RPGResource resource)
        {
            if (Options.BackupOnSaving)
            {
                await CreateBackupAsync(resource);
            }
        }

        public Task<bool> CreateBackupAsync(RPGResource resource)
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
                        return Task.FromResult(false);
                    }
                    return Client.CreateBackupAsync((int)resource, backupPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't make backup", ex);
            }
            return Task.FromResult(false);
        }

        public void SaveSession()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(SessionFilePath, json);
        }

        public ISessionContext LoadSession()
        {
            string json = File.ReadAllText(SessionFilePath);
            DefaultSessionContext session = JsonConvert.DeserializeObject<DefaultSessionContext>(json);
            session.SessionFilePath = SessionFilePath;
            return session;
        }

    }
}
