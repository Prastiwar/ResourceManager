using MediatR;
using ResourceManager.Data;
using System;

namespace ResourceManager.Commands
{
    public class GetPresentableByIdQuery : IRequest<PresentableData>
    {
        public GetPresentableByIdQuery(Type resourceType, object id)
        {
            ResourceType = resourceType;
            Id = id;
        }

        public Type ResourceType { get; }
        public object Id { get; }
    }
}
