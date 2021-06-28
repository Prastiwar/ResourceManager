using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public IResourceDescriptorService DescriptorService => CurrentSource.DescriptorService;

        protected IServiceCollection ActualServices { get; private set; }

        public void Configure(string name, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (Providers.TryGetValue(name, out IDataSourceProvider provider))
            {
                if (ActualServices != null)
                {
                    ServicesBuilder.Unregister(ActualServices, CurrentSource);
                }
                ActualServices = ServicesBuilder.Create();
                CurrentSource = provider.Provide(ActualServices, configuration);
                ServicesBuilder.Configure(ActualServices, CurrentSource);
            }
            else
            {
                throw new InvalidOperationException($"Provider for data source name {name} is not registered");
            }
        }

        public void SaveChanges() => CurrentSource.SaveChanges();
        public Task SaveChangesAsync(CancellationToken token) => CurrentSource.SaveChangesAsync(token);

        public IQueryable<object> Query(Type resourceType) => CurrentSource.Query(resourceType);
        public IQueryable<T> Query<T>() => CurrentSource.Query<T>();

        public TrackedResource<object> Attach(object resource, Type resourceType) => CurrentSource.Attach(resource, resourceType);
        public TrackedResource<T> Attach<T>(T resource) => CurrentSource.Attach(resource);

        public TrackedResource<object> Add(object resource, Type resourceType) => CurrentSource.Add(resource, resourceType);
        public Task<TrackedResource<object>> AddAsync(object resource, Type resourceType) => CurrentSource.AddAsync(resource, resourceType);
        public TrackedResource<T> Add<T>(T resource) => CurrentSource.Add(resource);
        public Task<TrackedResource<T>> AddAsync<T>(T resource) => CurrentSource.AddAsync(resource);

        public TrackedResource<object> Update(object resource, Type resourceType) => CurrentSource.Update(resource, resourceType);
        public Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType) => CurrentSource.UpdateAsync(resource, resourceType);
        public TrackedResource<T> Update<T>(T resource) => CurrentSource.Update(resource);
        public Task<TrackedResource<T>> UpdateAsync<T>(T resource) => CurrentSource.UpdateAsync(resource);

        public TrackedResource<object> Delete(object resource, Type resourceType) => CurrentSource.Delete(resource, resourceType);
        public Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType) => CurrentSource.DeleteAsync(resource, resourceType);
        public TrackedResource<T> Delete<T>(T resource) => CurrentSource.Delete(resource);
        public Task<TrackedResource<T>> DeleteAsync<T>(T resource) => CurrentSource.DeleteAsync(resource);
    }
}
