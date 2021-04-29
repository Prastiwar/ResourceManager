namespace RPGDataEditor.Core.Connection
{
    public class LocalConnectionConfig : ConnectionConfig
    {
        public string FolderPath {
            get => (string)Get();
            set => Set(value);
        }

        public string FileSearchPattern {
            get => (string)Get();
            set => Set(value);
        }
    }
}
