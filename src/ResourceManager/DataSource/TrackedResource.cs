using System;

namespace ResourceManager.DataSource
{
    public class TrackedResource
    {
        public TrackedResource(TrackingEntry entry) => Entry = entry;

        public TrackingEntry Entry { get; }

        public object OriginalResource => Entry.OriginalResource;

        public Type ResourceType => Entry.ResourceType;

        public object Resource {
            get => Entry.Resource;
            protected set => Entry.Resource = value;
        }

        public ResourceState State {
            get => Entry.State;
            protected set => Entry.State = value;
        }

        /// <summary> Marks resource to ignore it. When IDataSource.SaveChanges or IDataSource.SaveChangesAsync is called, nothing will happen with this resource. </summary>
        public void Unchange()
        {
            State = ResourceState.Unchanged;
            Resource = OriginalResource;
        }

        /// <summary> Marks resource to update. Call IDataSource.SaveChanges or IDataSource.SaveChangesAsync to make this change happen. </summary>
        public void Update() => State = ResourceState.Modified;

        /// <summary> Marks resource to update. Call IDataSource.SaveChanges or IDataSource.SaveChangesAsync to make this change happen. </summary>
        public void Update(object resource)
        {
            if (resource.GetType() != OriginalResource.GetType())
            {
                throw new InvalidOperationException($"Cannot update original resource of type {OriginalResource.GetType()} with object of type {resource.GetType()}");
            }
            Resource = resource;
            Update();
        }

        /// <summary> Marks resource to delete. Call IDataSource.SaveChanges or IDataSource.SaveChangesAsync to make this change happen. </summary>
        public void Delete()
        {
            State = ResourceState.Removed;
            Resource = OriginalResource;
        }

        /// <summary> Marks resource to be inserted. Call IDataSource.SaveChanges or IDataSource.SaveChangesAsync to make this change happen. </summary>
        public void Create() => State = ResourceState.Added;
    }

    public class TrackedResource<T> : TrackedResource
    {
        public TrackedResource(TrackingEntry entry) : base(entry) { }

        public new T OriginalResource => (T)base.OriginalResource;

        public new T Resource => (T)base.Resource;

        /// <summary> Marks resource to update. Call IDataSource.SaveChanges or IDataSource.SaveChangesAsync to make this change happen. </summary>
        public void Update(T resource)
        {
            base.Resource = resource;
            Update();
        }
    }
}
