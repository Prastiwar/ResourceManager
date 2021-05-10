using MediatR;
using RPGDataEditor.Models;
using System;

namespace RPGDataEditor.Commands
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

    public class GetPresentableByIdQuery<TResource> : GetPresentableByIdQuery
    {
        public GetPresentableByIdQuery(object id) : base(typeof(TResource), id) { }
    }
}
