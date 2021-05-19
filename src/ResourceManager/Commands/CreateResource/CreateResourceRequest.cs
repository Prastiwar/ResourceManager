using System;

namespace ResourceManager.Commands
{
    public sealed class CreateResourceRequest : ResourceRequest<CreateResourceResults>
    {
        public CreateResourceRequest(Type resourceType, object resource) : base(resourceType, resource) { }
    }
}
