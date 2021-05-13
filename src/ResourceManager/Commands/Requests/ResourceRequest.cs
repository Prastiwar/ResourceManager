using MediatR;
using System;

namespace ResourceManager.Commands
{
    public abstract class ResourceRequest<TResults> : IRequest<TResults>
    {
        public ResourceRequest(Type resourceType, object resource)
        {
            ResourceType = resourceType;
            Resource = resource;
        }

        public Type ResourceType { get; }
        public object Resource { get; }
    }
}
