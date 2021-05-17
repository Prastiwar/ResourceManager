using Microsoft.Extensions.Configuration;

namespace ResourceManager
{
    public static class ConfigurationExtensions
    {
        private const string DataSourceKey = "DataSource";

        public static IConfigurationSection GetDataSourceSection(this IConfiguration configuration)
        {
            if (configuration is IConfigurationSection section && section.Key == DataSourceKey)
            {
                return section;
            }
            return configuration.GetSection(DataSourceKey);
        }

        public static void SetDataSource(this IConfiguration configuration, string name)
        {
            IConfigurationSection section = GetDataSourceSection(configuration);
            section["Type"] = name;
        }
    }
}
