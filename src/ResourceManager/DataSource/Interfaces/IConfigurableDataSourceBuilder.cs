using Microsoft.Extensions.DependencyInjection;

namespace ResourceManager.DataSource
{
    public interface IConfigurableDataSourceBuilder
    {
        IServiceCollection Services { get; }

        IConfigurableDataSourceBuilder Add(string name, IDataSourceProvider provider);

        IConfigurableDataSource Build();
    }
}
