using MediatR;
using System;

namespace ResourceManager.Commands
{
    public class GetResourceByIdQuery : IRequest<object>
    {
        public GetResourceByIdQuery(Type resourceType, object id = null)
        {
            ResourceType = resourceType;
            Id = id;
        }

        public Type ResourceType { get; }
        public object Id { get; }
    }

    public class GetResourceByIdQuery<TResource> : IRequest<TResource>
    {
        public GetResourceByIdQuery(object id = null) => Id = id;

        public object Id { get; }
    }
}
