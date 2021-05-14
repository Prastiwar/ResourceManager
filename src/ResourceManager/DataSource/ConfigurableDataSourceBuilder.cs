using System;
using System.Collections.Generic;

namespace ResourceManager.DataSource
{
    public class ConfigurableDataSourceBuilder : IConfigurableDataSourceBuilder
    {
        protected IDictionary<string, IDataSourceProvider> ProvidersLookup { get; } = new Dictionary<string, IDataSourceProvider>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<KeyValuePair<string, IDataSourceProvider>> Providers => ProvidersLookup;

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
            return new ConfigurableDataSource(Providers);
        }
    }
}
