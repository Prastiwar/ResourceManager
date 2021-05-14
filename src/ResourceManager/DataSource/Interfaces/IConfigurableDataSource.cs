using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IConfigurableDataSource : IDataSource
    {
        void Configure(string name, IConfiguration configuration);
    }
}
