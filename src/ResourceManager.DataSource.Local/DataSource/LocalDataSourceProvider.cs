using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ResourceManager.DataSource.Local.Configuration;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSourceProvider : IDataSourceProvider
    {
        public LocalDataSourceProvider(LocalDataSourceBuilderOptions builderOptions) => BuilderOptions = builderOptions;

        protected LocalDataSourceBuilderOptions BuilderOptions { get; }

        public IDataSource Provide(IConfiguration configuration)
        {
            LocalConnectionMonitor monitor = new LocalConnectionMonitor();
            IOptions<LocalDataSourceOptions> options = Options.Create(new LocalDataSourceOptions() {
                FileSearchPattern = configuration[nameof(BuilderOptions.FileSearchPattern)] ?? BuilderOptions.FileSearchPattern,
                FolderPath = configuration[nameof(BuilderOptions.FolderPath)] ?? BuilderOptions.FolderPath,
            });
            return new LocalDataSource(configuration, monitor, options);
        }
    }
}
