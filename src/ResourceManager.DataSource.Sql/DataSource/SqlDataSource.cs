using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
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

        protected override TrackedResource<T> GetOrCreateTracked<T>(T resource, ResourceState state, Type asType = null)
        {
            EntityEntry entityEntry = DbContext.ChangeTracker.Entries().FirstOrDefault(x => x is T entity && EqualityComparer<T>.Default.Equals(entity, resource));
            if (entityEntry == null)
            {
                entityEntry = DbContext.Entry(resource);
            }
            EntityState newState = state switch {
                ResourceState.Unchanged => EntityState.Unchanged,
                ResourceState.Added => EntityState.Added,
                ResourceState.Modified => EntityState.Modified,
                ResourceState.Removed => EntityState.Deleted,
                _ => throw new InvalidOperationException()
            };
            if (entityEntry.State != newState)
            {
                entityEntry.State = newState;
            }
            TrackingEntry entry = new EntityTrackingEntry(entityEntry);
            TrackedResources.Add(entry);
            return new TrackedResource<T>(entry);
        }
    }
}
