using System;

namespace ResourceManager.Data
{
    public class ResourceDescriptor : IResourceDescriptor
    {
        public ResourceDescriptor(Type type) => Type = type;

        public Type Type { get; protected set; }

        public virtual bool CanDescribe<T>(T resource) => resource != null && CanDescribe(resource.GetType());
        public virtual bool CanDescribe(Type resourceType) => Type.IsAssignableFrom(resourceType);
    }
}
