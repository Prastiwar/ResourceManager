using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IDataSourceProvider
    {
        IDataSource Provide(IConfiguration configuration);
    }
}
