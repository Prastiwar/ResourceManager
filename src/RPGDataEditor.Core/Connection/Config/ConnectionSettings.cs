namespace RPGDataEditor.Core.Connection
{
    public interface IConnectionSettings
    {
        IConnectionConfig Config { get; set; }
    }

    public class ConnectionSettings : IConnectionSettings
    {
        public IConnectionConfig Config { get; set; }
    }
}
