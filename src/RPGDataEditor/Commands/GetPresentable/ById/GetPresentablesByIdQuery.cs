using MediatR;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Commands
{
    public class GetPresentablesByIdQuery : IRequest<IEnumerable<PresentableData>>
    {
        public GetPresentablesByIdQuery(Type resourceType, object[] ids)
        {
            ResourceType = resourceType;
            Ids = ids;
        }

        public Type ResourceType { get; }
        public object[] Ids { get; }
    }

    public class GetPresentablesByIdQuery<TResource> : GetPresentablesByIdQuery
    {
        public GetPresentablesByIdQuery(object[] ids) : base(typeof(TResource), ids) { }
    }
}
