using ResourceManager.DataSource.Local;
using System;

namespace ResourceManager.DataSource
{
    public static class LocalDataSourceExtensions
    {
        public const string Name = "Local";

        public static IConfigurableDataSourceBuilder AddLocalDataSource(this IConfigurableDataSourceBuilder builder, Action<LocalDataSourceBuilderOptions> options)
        {
            LocalDataSourceBuilderOptions builderOptions = new LocalDataSourceBuilderOptions {
                Name = Name,
                FileSearchPattern = "*.json"
            };
            options?.Invoke(builderOptions);
            LocalDataSourceProvider provider = new LocalDataSourceProvider(builderOptions);
            return builder.Add(builderOptions.Name, provider);
        }
    }
}
