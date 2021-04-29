namespace RPGDataEditor.Core.Connection
{
    public class SqlConnectionConfig : ConnectionConfig
    {
        public string ConnectionString {
            get => (string)Get();
            set => Set(value);
        }
    }
}
