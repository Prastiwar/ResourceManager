using System;

namespace ResourceManager.Commands
{
    public class GetResourcesByPathQuery : EnumerableResourceQuery
    {
        public GetResourcesByPathQuery(Type resourceType, string path) : base(resourceType) => Path = path;

        public string Path { get; }
    }
}
