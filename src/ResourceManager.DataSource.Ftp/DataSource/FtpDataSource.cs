using FluentFTP;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Ftp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSource : IDataSource
    {
        public FtpDataSource(IConfiguration configuration, IConnectionMonitor monitor, FtpDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IConnectionMonitor Monitor { get; }

        public FtpDataSourceOptions Options { get; }

        public class ResourcesEntry
        {
            public IList<FtpListItem> Files { get; set; }

            public IList<object> Resources { get; set; }

            //public CachingPolicy CachingPolicy { get; set; }
        }

        private IDictionary<Type, ResourcesEntry> entries = new Dictionary<Type, ResourcesEntry>();

        public IQueryable<T> Query<T>()
        {
            throw new NotImplementedException();
            //if (entries.TryGetValue(typeof(T), out ResourcesEntry entry))
            //{
            //    if (!CachingPolicy.IsExpired())
            //    {
            //        return entry.Resources.Cast<T>().AsQueryable();
            //    }
            //}
            //entry = new ResourcesEntry();
            //FtpClient c = new FtpClient();
            //entry.Files = c.GetListingAsync().Result.Select(x => x.FullName).ToList();
        }

        public void SaveChanges() => throw new NotImplementedException();
        public Task SaveChangesAsync(CancellationToken token) => throw new NotImplementedException();
        public IQueryable<object> Query(Type resourceType) => throw new NotImplementedException();
        public TrackedResource<object> Add(object resource, Type resourceType) => throw new NotImplementedException();
        public Task<TrackedResource<object>> AddAsync(object resource, Type resourceType) => throw new NotImplementedException();
        public TrackedResource<T> Add<T>(T resource) => throw new NotImplementedException();
        public Task<TrackedResource<T>> AddAsync<T>(T resource) => throw new NotImplementedException();
        public TrackedResource<object> Update(object resource, Type resourceType) => throw new NotImplementedException();
        public Task<TrackedResource<object>> UpdateAsync(object resource, Type resourceType) => throw new NotImplementedException();
        public TrackedResource<T> Update<T>(T resource) => throw new NotImplementedException();
        public Task<TrackedResource<T>> UpdateAsync<T>(T resource) => throw new NotImplementedException();
        public TrackedResource<object> Delete(object resource, Type resourceType) => throw new NotImplementedException();
        public Task<TrackedResource<object>> DeleteAsync(object resource, Type resourceType) => throw new NotImplementedException();
        public TrackedResource<T> Delete<T>(T resource) => throw new NotImplementedException();
        public Task<TrackedResource<T>> DeleteAsync<T>(T resource) => throw new NotImplementedException();
    }
}
