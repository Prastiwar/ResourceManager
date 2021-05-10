using MediatR;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Commands
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

    public class GetPresentablesByPathQuery<TResource> : GetPresentablesByPathQuery
    {
        public GetPresentablesByPathQuery(string[] paths) : base(typeof(TResource), paths) { }
    }
}
