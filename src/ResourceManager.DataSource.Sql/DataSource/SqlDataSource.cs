using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.DataSource.Sql.Data;
using ResourceManager.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql
{
    public class SqlDataSource : DataSource
    {
        public SqlDataSource(IConfiguration configuration, IConnectionMonitor monitor, IResourceDescriptorService descriptorService, DbContext dbContext, SqlDataSourceOptions options)
            : base(configuration, monitor, descriptorService)
        {
            DbContext = dbContext;
            Options = options;
        }

        public SqlDataSourceOptions Options { get; }

        protected DbContext DbContext { get; }

        public override void SaveChanges() => DbContext.SaveChanges();

        public override Task SaveChangesAsync(CancellationToken token) => DbContext.SaveChangesAsync(token);

        public override IQueryable<object> Query(Type resourceType) => DbContext.Set(resourceType);

        public override IQueryable<string> Locate(Type resourceType)
        {
            SqlLocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<SqlLocationResourceDescriptor>(resourceType);
            // TODO: Improve performance: Calling ToArray() is not good since it loads every resources to memory
            object[] resources = Query(resourceType).ToArray();
            return resources.Select(resource => descriptor.GetRelativeFullPath(resource)).AsQueryable();
        }

        protected override TrackedResource<T> CreateTracked<T>(T resource, ResourceState state, Type asType = null)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry = DbContext.Entry(resource);
            entry.State = state switch {
                ResourceState.Unchanged => EntityState.Unchanged,
                ResourceState.Added => EntityState.Added,
                ResourceState.Modified => EntityState.Modified,
                ResourceState.Removed => EntityState.Deleted,
                _ => throw new InvalidOperationException()
            };
            TrackedResource<T> tracked = new TrackedResource<T>(resource, state, asType);
            TrackedResources.Add(tracked);
            return tracked;
        }
    }
}
