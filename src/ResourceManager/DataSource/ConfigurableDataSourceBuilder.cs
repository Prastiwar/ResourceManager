using Microsoft.Extensions.DependencyInjection;
using ResourceManager.Data;
using ResourceManager.Services;
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

        protected IList<Type> RegisteredResourceTypes { get; } = new List<Type>();

        public void RegisterResourceTypes(params Type[] types) => RegisteredResourceTypes.AddRange(types);

        public IConfigurableDataSourceBuilder Add(string name, IDataSourceProvider provider)
        {
            ValidateProvider(provider);
            ProvidersLookup[name] = provider;
            return this;
        }

        protected virtual void ValidateProvider(IDataSourceProvider provider)
        {
            IDataSourceProviderBuilderOptions options = provider.GetBuilderOptions();
            if (options.DescriptorService is null)
            {
                throw new ArgumentNullException(nameof(options.DescriptorService), $"{typeof(IDataSourceProvider)} must provide {typeof(IResourceDescriptorService)}");
            }
            foreach (Type resourceType in RegisteredResourceTypes)
            {
                try
                {
                    options.DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($" {typeof(ResourceDescriptor)} of type {typeof(LocationResourceDescriptor)} is required for resource {resourceType}", nameof(provider), ex);
                }
            }
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
