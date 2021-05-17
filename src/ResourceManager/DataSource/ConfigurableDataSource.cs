using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ResourceManager.DataSource
{
    public class ConfigurableDataSource : IConfigurableDataSource
    {
        public ConfigurableDataSource(IEnumerable<KeyValuePair<string, IDataSourceProvider>> providers)
            => Providers = new Dictionary<string, IDataSourceProvider>(providers, StringComparer.OrdinalIgnoreCase);

        protected IDictionary<string, IDataSourceProvider> Providers { get; }

        public IDataSource DataSource { get; protected set; }

        public IConfiguration Configuration => DataSource.Configuration;

        public IConnectionMonitor Monitor => DataSource.Monitor;

        public event EventHandler<ConfigurationChangedEventArgs> Changed;

        public void Configure(string name, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (Providers.TryGetValue(name, out IDataSourceProvider provider))
            {
                IDataSource oldDataSource = DataSource;
                DataSource = provider.Provide(configuration);
                Changed?.Invoke(this, new ConfigurationChangedEventArgs(oldDataSource, configuration));
            }
            else
            {
                throw new InvalidOperationException($"Provider for data source name {name} is not registered");
            }
        }
    }
}
