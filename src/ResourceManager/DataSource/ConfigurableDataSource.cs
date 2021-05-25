using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ResourceManager.DataSource
{
    public class ConfigurableDataSource : IConfigurableDataSource
    {
        public ConfigurableDataSource(IDataSourceServicesConfigurator servicesConfigurator, IEnumerable<KeyValuePair<string, IDataSourceProvider>> providers)
        {
            ServicesBuilder = servicesConfigurator ?? throw new ArgumentNullException(nameof(servicesConfigurator));
            Providers = new Dictionary<string, IDataSourceProvider>(providers, StringComparer.OrdinalIgnoreCase);
        }

        protected IDictionary<string, IDataSourceProvider> Providers { get; }

        protected IDataSourceServicesConfigurator ServicesBuilder { get; }

        public IDataSource CurrentSource { get; protected set; }

        public IConfiguration Configuration => CurrentSource.Configuration;

        public IConnectionMonitor Monitor => CurrentSource.Monitor;

        public void Configure(string name, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (Providers.TryGetValue(name, out IDataSourceProvider provider))
            {
                IServiceCollection services = ServicesBuilder.Create();
                CurrentSource = provider.Provide(services, configuration);
                ServicesBuilder.Configure(services, CurrentSource);
            }
            else
            {
                throw new InvalidOperationException($"Provider for data source name {name} is not registered");
            }
        }
    }
}
