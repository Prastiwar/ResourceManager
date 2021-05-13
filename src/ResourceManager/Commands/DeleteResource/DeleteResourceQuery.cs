using System;

namespace ResourceManager.Commands
{
    public sealed class DeleteResourceQuery : ResourceRequest<DeleteResourceResults>
    {
        public DeleteResourceQuery(Type resourceType, object resource) : base(resourceType, resource) { }
    }
}
