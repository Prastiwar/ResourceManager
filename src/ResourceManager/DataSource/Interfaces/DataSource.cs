using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource
{
    public abstract class DataSource : IDataSource
    {
        public IConfiguration Configuration { get; protected set; }

        public IConnectionMonitor Monitor { get; protected set; }

        protected IList<ITrackedResource> TrackedResources { get; } = new List<ITrackedResource>();

        public abstract void SaveChanges();

        public abstract Task SaveChangesAsync(CancellationToken token);

        public abstract IQueryable<object> Query(Type resourceType);

        public IQueryable<T> Query<T>() => Query(typeof(T)).Cast<T>();

        public abstract IQueryable<string> Locate(Type resourceType);

        public IQueryable<string> Locate<T>() => Locate(typeof(T));

        public TrackedResource<object> Attach(object resource, Type resourceType) => CreateTracked(resource, ResourceState.Unchanged, resourceType);
        public TrackedResource<T> Attach<T>(T resource) => CreateTracked(resource, ResourceState.Unchanged);

        public Task<TrackedResource<object>> AddAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Added, resourceType);
        public Task<TrackedResource<T>> AddAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Added);
        public TrackedResource<object> Add(object resource, Type resourceType) => CreateTracked(resource, ResourceState.Added, resourceType);
        public TrackedResource<T> Add<T>(T resource) => CreateTracked(resource, ResourceState.Added);

        public Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Modified, resourceType);
        public Task<TrackedResource<T>> UpdateAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Modified);
        public TrackedResource<object> Update(object resource, Type resourceType) => CreateTracked(resource, ResourceState.Modified, resourceType);
        public TrackedResource<T> Update<T>(T resource) => CreateTracked(resource, ResourceState.Modified);

        public Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType) => CreateTrackedAsync(resource, ResourceState.Removed, resourceType);
        public Task<TrackedResource<T>> DeleteAsync<T>(T resource) => CreateTrackedAsync(resource, ResourceState.Removed);
        public TrackedResource<object> Delete(object resource, Type resourceType) => CreateTracked(resource, ResourceState.Removed, resourceType);
        public TrackedResource<T> Delete<T>(T resource) => CreateTracked(resource, ResourceState.Removed);

        protected virtual TrackedResource<T> CreateTracked<T>(T resource, ResourceState state, Type asType = null)
        {
            TrackedResource<T> tracked = new TrackedResource<T>(resource, state, asType);
            TrackedResources.Add(tracked);
            return tracked;
        }

        protected virtual Task<TrackedResource<T>> CreateTrackedAsync<T>(T resource, ResourceState state, Type asType = null) => Task.FromResult(CreateTracked(resource, state, asType));
    }
}
