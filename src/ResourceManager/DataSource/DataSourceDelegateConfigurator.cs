using Microsoft.Extensions.DependencyInjection;
using System;

namespace ResourceManager.DataSource
{
    public class DataSourceDelegateConfigurator : IDataSourceServicesConfigurator
    {
        public DataSourceDelegateConfigurator(Action<IServiceCollection, IDataSource> action) => Action = action;

        protected Action<IServiceCollection, IDataSource> Action { get; }

        public IServiceCollection Create() => new ServiceCollection();

        public void Configure(IServiceCollection services, IDataSource currentSource) => Action?.Invoke(services, currentSource);
    }
}
