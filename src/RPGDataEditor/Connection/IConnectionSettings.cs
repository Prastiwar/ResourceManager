namespace RPGDataEditor.Connection
{
    public interface IConnectionSettings
    {
        IConnectionConfig CreateConfig();

        object Get(string parameter);

        void Set(string parameter, object value);

        bool Clear(string parameter);

        void Reset();
    }
}
