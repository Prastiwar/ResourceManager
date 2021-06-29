using Microsoft.Extensions.Configuration;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource
{
    public abstract class DataSource : IDataSource
    {
        public DataSource(IConfiguration configuration, IConnectionMonitor monitor, IResourceDescriptorService descriptorService)
        {
            Configuration = configuration;
            Monitor = monitor;
            DescriptorService = descriptorService;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public IResourceDescriptorService DescriptorService { get; }

        protected IList<TrackingEntry> TrackedResources { get; } = new List<TrackingEntry>();

        public abstract void SaveChanges();

        public abstract Task SaveChangesAsync(CancellationToken token);

        public abstract IQueryable<object> Query(Type resourceType);

        public IQueryable<T> Query<T>() => Query(typeof(T)).Cast<T>();

        public TrackedResource<object> Attach(object resource, Type resourceType) => GetOrCreateTracked(resource, ResourceState.Unchanged, resourceType);
        public TrackedResource<T> Attach<T>(T resource) => GetOrCreateTracked(resource, ResourceState.Unchanged);

        public Task<TrackedResource<object>> AddAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Added, resourceType);
        public Task<TrackedResource<T>> AddAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Added);
        public TrackedResource<object> Add(object resource, Type resourceType) => GetOrCreateTracked(resource, ResourceState.Added, resourceType);
        public TrackedResource<T> Add<T>(T resource) => GetOrCreateTracked(resource, ResourceState.Added);

        public Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Modified, resourceType);
        public Task<TrackedResource<T>> UpdateAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Modified);
        public TrackedResource<object> Update(object resource, Type resourceType) => GetOrCreateTracked(resource, ResourceState.Modified, resourceType);
        public TrackedResource<T> Update<T>(T resource) => GetOrCreateTracked(resource, ResourceState.Modified);

        public Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Removed, resourceType);
        public Task<TrackedResource<T>> DeleteAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Removed);
        public TrackedResource<object> Delete(object resource, Type resourceType) => GetOrCreateTracked(resource, ResourceState.Removed, resourceType);
        public TrackedResource<T> Delete<T>(T resource) => GetOrCreateTracked(resource, ResourceState.Removed);

        protected virtual TrackedResource<T> GetOrCreateTracked<T>(T resource, ResourceState state, Type asType = null)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }
            TrackingEntry foundTrackingEntry = TrackedResources.FirstOrDefault(t => t.Resource is T res && EqualityComparer<T>.Default.Equals(res, resource));
            if (foundTrackingEntry != null)
            {
                TrackedResource<T> trackedResource = new TrackedResource<T>(foundTrackingEntry);
                if (trackedResource.State != state)
                {
                    switch (state)
                    {
                        case ResourceState.Unchanged:
                            trackedResource.Unchange();
                            break;
                        case ResourceState.Added:
                            trackedResource.Create();
                            break;
                        case ResourceState.Modified:
                            trackedResource.Update();
                            break;
                        case ResourceState.Removed:
                            trackedResource.Delete();
                            break;
                        default:
                            break;
                    }
                }
                return trackedResource;
            }
            TrackingEntry entry = new TrackingEntry(resource, state, asType);
            TrackedResources.Add(entry);
            return new TrackedResource<T>(entry);
        }

        protected virtual Task<TrackedResource<T>> CreateTrackedAsync<T>(T resource, ResourceState state, Type asType = null) => Task.FromResult(GetOrCreateTracked(resource, state, asType));
    }
}
