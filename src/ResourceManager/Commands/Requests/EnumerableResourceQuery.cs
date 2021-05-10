using System;

namespace ResourceManager.Commands
{
    public class EnumerableResourceQuery : IEnumerableResourceQuery
    {
        public EnumerableResourceQuery(Type type) => ResourceType = type;

        public Type ResourceType { get; }
    }
}
