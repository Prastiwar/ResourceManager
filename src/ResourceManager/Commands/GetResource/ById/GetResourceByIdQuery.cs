using System;

namespace ResourceManager.Commands
{
    public sealed class GetResourceByIdQuery : ResourceQuery
    {
        public GetResourceByIdQuery(Type resourceType, object id) : base(resourceType) => Id = id;

        public object Id { get; }
    }
}
