using System;

namespace ResourceManager.Commands
{
    public class GetResourceByIdQuery : ResourceQuery
    {
        public GetResourceByIdQuery(Type resourceType, object id) : base(resourceType) => Id = id;

        public object Id { get; }
    }

    public class GetResourceByIdQuery<TResource> : GetResourceByIdQuery
    {
        public GetResourceByIdQuery(object id) : base(typeof(TResource), id) { }
    }
}
