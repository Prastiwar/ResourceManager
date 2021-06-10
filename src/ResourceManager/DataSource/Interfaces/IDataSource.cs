using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource
{
    public interface IDataSource
    {
        IConfiguration Configuration { get; }

        IConnectionMonitor Monitor { get; }

        void SaveChanges();
        Task SaveChangesAsync(CancellationToken token);

        IQueryable<object> Query(Type resourceType);
        IQueryable<T> Query<T>();

        IQueryable<string> Locate(Type resourceType);
        IQueryable<string> Locate<T>();

        TrackedResource<object> Attach(object resource, Type resourceType);
        TrackedResource<T> Attach<T>(T resource);

        TrackedResource<object> Add(object resource, Type resourceType);
        Task<TrackedResource<object>> AddAsync(object resource, Type resourceType);
        TrackedResource<T> Add<T>(T resource);
        Task<TrackedResource<T>> AddAsync<T>(T resource);

        TrackedResource<object> Update(object resource, Type resourceType);
        Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType);
        TrackedResource<T> Update<T>(T resource);
        Task<TrackedResource<T>> UpdateAsync<T>(T resource);

        TrackedResource<object> Delete(object resource, Type resourceType);
        Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType);
        TrackedResource<T> Delete<T>(T resource);
        Task<TrackedResource<T>> DeleteAsync<T>(T resource);
    }
}
