using System;
using System.Collections.Generic;

namespace ResourceManager.DataSource
{
    public abstract class DataSourceConfigurator : IDataSourceConfigurator
    {
        protected IDictionary<string, IDataSourceProvider> Providers { get; } = new Dictionary<string, IDataSourceProvider>();

        public void Configure(IDataSourceConfiguration configuration)
        {
            if(Providers.TryGetValue(configuration.Type, out IDataSourceProvider provider))
            {
                provider.Configure(configuration.Configuration);

            }
            else
            {
                throw new InvalidOperationException($"Provider for data source type {configuration.Type} is not registered");
            }
        }
    }
}
