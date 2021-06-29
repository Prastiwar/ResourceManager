using System;

namespace ResourceManager.DataSource
{
    public class TrackingEntry
    {
        public TrackingEntry(object resource, ResourceState state, Type asType = null)
            : this(resource.DeepClone(), resource, state, asType ?? resource.GetType()) { }

        public TrackingEntry(object originalResource, object resource, ResourceState state, Type asType)
        {
            OriginalResource = originalResource;
            Resource = resource;
            State = state;
            ResourceType = asType;
        }

        protected TrackingEntry() { }

        public virtual object OriginalResource { get; }

        public virtual Type ResourceType { get; }

        public virtual object Resource { get; set; }

        public virtual ResourceState State { get; set; }
    }
}
