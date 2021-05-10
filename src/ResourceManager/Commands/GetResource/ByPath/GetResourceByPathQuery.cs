using System;

namespace ResourceManager.Commands
{
    public class GetResourceByPathQuery : ResourceQuery
    {
        public GetResourceByPathQuery(Type resourceType, string path) : base(resourceType) => Path = path;

        public string Path { get; }
    }
}
