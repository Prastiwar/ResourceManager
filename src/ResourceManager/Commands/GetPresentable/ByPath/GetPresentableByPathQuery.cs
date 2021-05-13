using MediatR;
using ResourceManager.Data;
using System;

namespace ResourceManager.Commands
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
}
