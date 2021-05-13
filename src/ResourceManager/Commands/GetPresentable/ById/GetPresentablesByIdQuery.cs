using MediatR;
using ResourceManager.Data;
using System;
using System.Collections.Generic;

namespace ResourceManager.Commands
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
}
