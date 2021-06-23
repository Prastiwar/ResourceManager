using System;

namespace ResourceManager.DataSource
{
    public class TrackedResource<T> : ITrackedResource
    {
        public TrackedResource(T resource, ResourceState state, Type asType = null)
        {
            OriginalResource = resource;
            Resource = resource;
            State = state;
            ResourceType = asType ?? typeof(T);
        }

        public T OriginalResource { get; }

        public Type ResourceType { get; private set; }

        public T Resource { get; private set; }

        public ResourceState State { get; private set; }

        object ITrackedResource.Resource => Resource;

        object ITrackedResource.OriginalResource => OriginalResource;

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

        public void Delete()
        {
            State = ResourceState.Removed;
            Resource = OriginalResource;
        }
    }
}
