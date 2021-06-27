using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IConfigurableDataSource : IDataSource
    {
        IDataSource CurrentSource { get; }

        void Configure(string name, IConfiguration configuration);
    }
}
