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
            if (builderOptions.Serializer is null)
            {
                throw new ArgumentNullException(nameof(builderOptions.Serializer), $"Yout must provide {typeof(Data.ITextSerializer)} to use this provider");
            }
            if (builderOptions.FolderPath is null)
            {
                throw new ArgumentNullException(nameof(builderOptions.FolderPath), $"Yout must provide {nameof(builderOptions.FolderPath)} to use this provider");
            }
            LocalDataSourceProvider provider = new LocalDataSourceProvider(builderOptions);
            return builder.Add(builderOptions.Name, provider);
        }
    }
}
