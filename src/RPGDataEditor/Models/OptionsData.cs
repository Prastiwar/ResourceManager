namespace RPGDataEditor.Models
{
    public class OptionsData
    {
        private string npcBackupPath = "";
        public string NpcBackupPath {
            get => npcBackupPath;
            set => npcBackupPath = value ?? "";
        }

        private string dialoguesBackupPath = "";
        public string DialoguesBackupPath {
            get => dialoguesBackupPath;
            set => dialoguesBackupPath = value ?? "";
        }

        private string questsBackupPath = "";
        public string QuestsBackupPath {
            get => questsBackupPath;
            set => questsBackupPath = value ?? "";
        }

        public bool PrettyPrint { get; set; }

        public bool BackupOnSaving { get; set; }
    }
}
