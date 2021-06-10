using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.DataSource.File;
using ResourceManager.DataSource.Local.Configuration;
using ResourceManager.DataSource.Local.Data;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSourceProvider : IDataSourceProvider
    {
        public LocalDataSourceProvider(LocalDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected LocalDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSource Provide(IServiceCollection services, IConfiguration configuration)
        {
            LocalConnectionMonitor monitor = new LocalConnectionMonitor();
            LocalDataSourceOptions options = new LocalDataSourceOptions() {
                FileSearchPattern = configuration[nameof(BuilderOptions.FileSearchPattern)] ?? BuilderOptions.FileSearchPattern,
                FolderPath = configuration[nameof(BuilderOptions.FolderPath)] ?? BuilderOptions.FolderPath,
            };
            services.AddSingleton<IFileClient>(new LocalFileClient(options));
            LocalDataSource dataSource = new LocalDataSource(configuration, monitor, BuilderOptions.DescriptorService, options);
            services.AddSingleton(dataSource);
            return dataSource;
        }
    }
}
