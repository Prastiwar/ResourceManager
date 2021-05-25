using Microsoft.Extensions.DependencyInjection;

namespace ResourceManager.DataSource
{
    public interface IDataSourceServicesConfigurator
    {
        IServiceCollection Create();

        void Configure(IServiceCollection services, IDataSource currentSource);
    }
}
