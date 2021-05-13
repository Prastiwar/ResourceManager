using System;

namespace ResourceManager.Commands
{
    public sealed class CreateResourceQuery : ResourceRequest<CreateResourceResults>
    {
        public CreateResourceQuery(Type resourceType, object resource) : base(resourceType, resource) { }
    }
}
