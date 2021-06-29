using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace ResourceManager.DataSource.Sql
{
    public class EntityTrackingEntry : TrackingEntry
    {
        public EntityTrackingEntry(EntityEntry entityEntry) => EntityEntry = entityEntry;

        public EntityEntry EntityEntry { get; }

        public override object OriginalResource => base.OriginalResource;

        public override object Resource {
            get => EntityEntry.Entity;
            set => EntityEntry.CurrentValues.SetValues(value);
        }

        public override Type ResourceType => EntityEntry.Metadata.ClrType;

        public override ResourceState State {
            get => EntityEntry.State switch {
                EntityState.Unchanged => ResourceState.Unchanged,
                EntityState.Detached => ResourceState.Unchanged,
                EntityState.Added => ResourceState.Added,
                EntityState.Modified => ResourceState.Modified,
                EntityState.Deleted => ResourceState.Removed,
                _ => throw new InvalidOperationException()
            };
            set => EntityEntry.State = value switch {
                ResourceState.Unchanged => EntityState.Unchanged,
                ResourceState.Added => EntityState.Added,
                ResourceState.Modified => EntityState.Modified,
                ResourceState.Removed => EntityState.Deleted,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
