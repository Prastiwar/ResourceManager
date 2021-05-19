using System;

namespace ResourceManager.Commands
{
    public sealed class DeleteResourceRequest : ResourceRequest<DeleteResourceResults>
    {
        public DeleteResourceRequest(Type resourceType, object resource) : base(resourceType, resource) { }
    }
}
