using System;

namespace ResourceManager.Commands
{
    public abstract class EnumerableResourceQuery : IEnumerableResourceQuery
    {
        public EnumerableResourceQuery(Type type) => ResourceType = type;

        public Type ResourceType { get; }
    }
}
