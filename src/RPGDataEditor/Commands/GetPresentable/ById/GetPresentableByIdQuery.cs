using MediatR;
using System;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByIdQuery : IRequest<object>
    {
        public GetPresentableByIdQuery(Type resourceType, object id = null)
        {
            ResourceType = resourceType;
            Id = id;
        }

        public Type ResourceType { get; }
        public object Id { get; }
    }

    public class GetPresentableByIdQuery<TResource> : IRequest<TResource>
    {
        public GetPresentableByIdQuery(object id = null) => Id = id;

        public object Id { get; }
    }
}
