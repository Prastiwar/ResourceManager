using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public interface IDataSourceProvider
    {
        void Configure(IConfiguration configuration);

        IConnectionMonitor CreateMonitor();
    }
}
