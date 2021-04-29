namespace RPGDataEditor.Core.Connection
{
    public interface IConnectionConfig
    {
        object Get(string parameter);

        void Set(string parameter, object value);
    }
}
