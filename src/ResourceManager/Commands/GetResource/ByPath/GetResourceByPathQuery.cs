using System;

namespace ResourceManager.Commands
{
    public sealed class GetResourceByPathQuery : ResourceQuery
    {
        public GetResourceByPathQuery(Type resourceType, string path) : base(resourceType) => Path = path;

        public string Path { get; }
    }
}
