using Microsoft.Extensions.DependencyInjection;
using System;

namespace ResourceManager.DataSource
{
    public interface IConfigurableDataSourceBuilder
    {
        IServiceCollection Services { get; }

        IConfigurableDataSourceBuilder Add(string name, IDataSourceProvider provider);

        void RegisterResourceTypes(params Type[] types);

        IConfigurableDataSource Build();
    }
}
