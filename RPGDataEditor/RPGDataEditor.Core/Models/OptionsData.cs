namespace RPGDataEditor.Core.Models
{
    public class OptionsData : ObservableModel
    {
        private string npcBackupPath = "";
        public string NpcBackupPath {
            get => npcBackupPath;
            set => SetProperty(ref npcBackupPath, value ?? "");
        }

        private string dialoguesBackupPath = "";
        public string DialoguesBackupPath {
            get => dialoguesBackupPath;
            set => SetProperty(ref dialoguesBackupPath, value ?? "");
        }

        private string questsBackupPath = "";
        public string QuestsBackupPath {
            get => questsBackupPath;
            set => SetProperty(ref questsBackupPath, value ?? "");
        }

        private bool prettyPrint = true;
        public bool PrettyPrint {
            get => prettyPrint;
            set => SetProperty(ref prettyPrint, value);
        }

        private bool backupOnSaving;
        public bool BackupOnSaving {
            get => backupOnSaving;
            set => SetProperty(ref backupOnSaving, value);
        }
    }
}
