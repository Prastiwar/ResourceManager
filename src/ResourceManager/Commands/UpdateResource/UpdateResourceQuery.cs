using System;

namespace ResourceManager.Commands
{
    public sealed class UpdateResourceQuery : ResourceRequest<UpdateResourceResults>
    {
        public UpdateResourceQuery(Type resourceType, object oldResource, object resource) : base(resourceType, resource) => OldResource = oldResource;

        public object OldResource { get; }
    }
}
