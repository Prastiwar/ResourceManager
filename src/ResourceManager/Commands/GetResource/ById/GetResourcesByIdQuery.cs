using System;

namespace ResourceManager.Commands
{
    public sealed class GetResourcesByIdQuery : EnumerableResourceQuery
    {
        public GetResourcesByIdQuery(Type resourceType, object[] ids) : base(resourceType) => Ids = ids;

        public object[] Ids { get; }
    }
}
