using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource
{
    public enum ResourceState
    {
        Unchanged,
        Added,
        Modified,
        Removed
    }

    public class TrackedResource<T>
    {
        public TrackedResource(T resource, ResourceState state)
        {
            OriginalResource = resource;
            Resource = resource;
        }

        protected T OriginalResource { get; }

        public T Resource { get; private set; }

        public ResourceState State { get; private set; }

        public void Unchange()
        {
            State = ResourceState.Unchanged;
            Resource = OriginalResource;
        }

        public void Update(T resource)
        {
            Resource = resource;
            State = ResourceState.Modified;
        }

        public void Delete() => State = ResourceState.Removed;
    }

    public interface IDataSource
    {
        IConfiguration Configuration { get; }

        IConnectionMonitor Monitor { get; }

        //void SaveChanges();

        //Task SaveChangesAsync(CancellationToken token);

        //IQueryable<object> Query(Type resourceType);
        //IQueryable<T> Query<T>();

        //TrackedResource<object> Add(object resource, Type resourceType);
        //Task<TrackedResource<object>> AddAsync(object resource, Type resourceType);
        //TrackedResource<T> Add<T>(T resource);
        //Task<TrackedResource<T>> AddAsync<T>(T resource);

        //TrackedResource<object> Update(object resource, Type resourceType);
        //Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType);
        //TrackedResource<T> Update<T>(T resource);
        //Task<TrackedResource<T>> UpdateAsync<T>(T resource);

        //TrackedResource<object> Delete(object resource, Type resourceType);
        //Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType);
        //TrackedResource<T> Delete<T>(T resource);
        //Task<TrackedResource<T>> DeleteAsync<T>(T resource);
    }
}
