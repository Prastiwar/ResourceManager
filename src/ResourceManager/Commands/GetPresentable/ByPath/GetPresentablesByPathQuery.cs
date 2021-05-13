using MediatR;
using ResourceManager.Data;
using System;
using System.Collections.Generic;

namespace ResourceManager.Commands
{
    public class GetPresentablesByPathQuery : IRequest<IEnumerable<PresentableData>>
    {
        public GetPresentablesByPathQuery(Type resourceType, string[] paths)
        {
            ResourceType = resourceType;
            Paths = paths;
        }

        public Type ResourceType { get; }
        public string[] Paths { get; }
    }
}
