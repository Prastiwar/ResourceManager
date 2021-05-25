using Microsoft.Extensions.DependencyInjection;

namespace ResourceManager.DataSource
{
    public class DataSourceWrapperConfigurator : IDataSourceServicesConfigurator
    {
        public DataSourceWrapperConfigurator(IServiceCollection collection) => Collection = collection;

        public IServiceCollection Collection { get; }

        public IServiceCollection Create() => Collection;

        public virtual void Configure(IServiceCollection services, IDataSource currentSource) { }
    }
}
