using ResourceManager.DataSource.Ftp;
using System;

namespace ResourceManager.DataSource
{
    public static class FtpDataSourceExtensions
    {
        public const string Name = "Ftp";

        public static IConfigurableDataSourceBuilder AddFtpDataSource(this IConfigurableDataSourceBuilder builder, Action<FtpDataSourceBuilderOptions> options = null)
        {
            FtpDataSourceBuilderOptions builderOptions = new FtpDataSourceBuilderOptions {
                Name = Name,
            };
            options?.Invoke(builderOptions);
            FtpDataSourceProvider provider = new FtpDataSourceProvider(builderOptions);
            return builder.Add(builderOptions.Name, provider);
        }
    }
}
