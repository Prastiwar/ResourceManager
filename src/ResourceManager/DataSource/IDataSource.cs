namespace ResourceManager.DataSource
{
    public interface IDataSource
    {
        IDataSourceConfiguration Configuration { get; }

        IConnectionMonitor Monitor { get; }
    }
}
