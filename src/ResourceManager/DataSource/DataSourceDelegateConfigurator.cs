using Microsoft.Extensions.DependencyInjection;
using System;

namespace ResourceManager.DataSource
{
    public class DataSourceDelegateConfigurator : IDataSourceServicesConfigurator
    {
        public DataSourceDelegateConfigurator(Action<IServiceCollection, IDataSource> configure) => ConfigureCall = configure;

        public DataSourceDelegateConfigurator(Action<IServiceCollection, IDataSource> configure, Action<IServiceCollection, IDataSource> unregister) : this(configure) => UnregisterCall = unregister;

        protected Action<IServiceCollection, IDataSource> ConfigureCall { get; }
        protected Action<IServiceCollection, IDataSource> UnregisterCall { get; }

        public IServiceCollection Create() => new ServiceCollection();

        public void Configure(IServiceCollection services, IDataSource currentSource) => ConfigureCall?.Invoke(services, currentSource);

        public void Unregister(IServiceCollection services, IDataSource previousSource) => UnregisterCall?.Invoke(services, previousSource);
    }
}
