namespace ResourceManager.DataSource
{
    public interface IDataSourceConfiguratorBuilder
    {
        IDataSourceConfiguratorBuilder Add(string name, IDataSourceProvider provider);

        IDataSourceConfigurator Build();
    }
}
