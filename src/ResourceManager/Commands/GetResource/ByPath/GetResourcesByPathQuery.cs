using System;

namespace ResourceManager.Commands
{
    public class GetResourcesByPathQuery : EnumerableResourceQuery
    {
        public GetResourcesByPathQuery(Type resourceType, string[] paths) : base(resourceType) => Paths = paths;

        public string[] Paths { get; }
    }
}
