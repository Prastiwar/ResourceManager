using ResourceManager.DataSource.Ftp;
using System;

namespace ResourceManager.DataSource
{
    public static class FtpDataSourceExtensions
    {
        public const string Name = "Ftp";

        public static IConfigurableDataSourceBuilder AddFtpDataSource(this IConfigurableDataSourceBuilder builder, Action<FtpDataSourceBuilderOptions> options)
        {
            FtpDataSourceBuilderOptions builderOptions = new FtpDataSourceBuilderOptions {
                Name = Name,
            };
            options?.Invoke(builderOptions);
            if (builderOptions.Serializer is null)
            {
                throw new ArgumentNullException(nameof(builderOptions.Serializer), $"Yout must provide {typeof(Data.ITextSerializer)} to use this provider");
            }
            FtpDataSourceProvider provider = new FtpDataSourceProvider(builderOptions);
            return builder.Add(builderOptions.Name, provider);
        }
    }
}
