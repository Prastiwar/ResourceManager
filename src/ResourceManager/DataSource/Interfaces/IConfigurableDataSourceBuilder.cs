namespace ResourceManager.DataSource
{
    public interface IConfigurableDataSourceBuilder
    {
        IConfigurableDataSourceBuilder Add(string name, IDataSourceProvider provider);

        IConfigurableDataSource Build();
    }
}
