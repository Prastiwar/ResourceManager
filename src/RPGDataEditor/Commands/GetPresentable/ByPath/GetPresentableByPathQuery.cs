using MediatR;
using RPGDataEditor.Models;
using System;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByPathQuery : IRequest<PresentableData>
    {
        public GetPresentableByPathQuery(Type resourceType, string path)
        {
            ResourceType = resourceType;
            Path = path;
        }

        public Type ResourceType { get; }
        public string Path { get; }
    }

    public class GetPresentableByPathQuery<TResource> : GetPresentableByPathQuery
    {
        public GetPresentableByPathQuery(string path) : base(typeof(TResource), path) { }
    }
}
