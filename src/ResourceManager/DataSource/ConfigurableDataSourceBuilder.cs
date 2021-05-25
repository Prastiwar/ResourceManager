using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ResourceManager.DataSource
{
    public class ConfigurableDataSourceBuilder : IConfigurableDataSourceBuilder
    {
        public ConfigurableDataSourceBuilder(IDataSourceServicesConfigurator servicesConfigurator, IServiceCollection services)
        {
            ServicesConfigurator = servicesConfigurator ?? throw new ArgumentNullException(nameof(servicesConfigurator));
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        protected IDictionary<string, IDataSourceProvider> ProvidersLookup { get; } = new Dictionary<string, IDataSourceProvider>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<KeyValuePair<string, IDataSourceProvider>> Providers => ProvidersLookup;

        public IServiceCollection Services { get; }

        public IDataSourceServicesConfigurator ServicesConfigurator { get; }

        public IConfigurableDataSourceBuilder Add(string name, IDataSourceProvider provider)
        {
            ProvidersLookup[name] = provider;
            return this;
        }

        public IConfigurableDataSource Build()
        {
            bool hasProviders = ProvidersLookup.Count > 0;
            if (!hasProviders)
            {
                throw new InvalidOperationException($"You must add providers to build {nameof(IConfigurableDataSource)}");
            }
            return new ConfigurableDataSource(ServicesConfigurator, Providers);
        }
    }
}
