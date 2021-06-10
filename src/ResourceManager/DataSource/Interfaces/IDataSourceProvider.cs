using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ResourceManager.DataSource
{
    public interface IDataSourceProvider
    {
        IDataSourceProviderBuilderOptions GetBuilderOptions();

        IDataSource Provide(IServiceCollection services, IConfiguration configuration);
    }
}
