using MediatR;
using System;

namespace ResourceManager.Commands
{
    public class RemoveCategoryQuery : IRequest<RemoveCategoryResults>
    {
        public RemoveCategoryQuery(Type resourceType, string category)
        {
            ResourceType = resourceType;
            Category = category;
        }

        public Type ResourceType { get; }
        public string Category { get; }
    }
}
