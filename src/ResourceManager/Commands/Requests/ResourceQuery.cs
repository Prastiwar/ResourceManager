using System;

namespace ResourceManager.Commands
{
    public abstract class ResourceQuery : IResourceQuery
    {
        public ResourceQuery(Type type) => ResourceType = type;

        public Type ResourceType { get; }
    }
}
