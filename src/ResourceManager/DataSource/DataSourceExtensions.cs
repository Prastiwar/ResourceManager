using Microsoft.Extensions.Configuration;

namespace ResourceManager.DataSource
{
    public static class DataSourceExtensions
    {
        public const string NameKey = "Type";

        public static void Configure(this IConfigurableDataSource configurator, IConfiguration configuration)
            => configurator.Configure(configuration[NameKey], configuration);
    }
}
