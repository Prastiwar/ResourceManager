using System;

namespace ResourceManager.Commands
{
    public class GetResourcesByIdQuery : EnumerableResourceQuery
    {
        public GetResourcesByIdQuery(Type resourceType, object[] ids) : base(resourceType) => Ids = ids;

        public object[] Ids { get; }
    }

    public class GetResourcesByIdQuery<TResource> : GetResourcesByIdQuery
    {
        public GetResourcesByIdQuery(object[] ids) : base(typeof(TResource), ids) { }
    }
}
