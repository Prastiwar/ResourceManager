using MediatR;
using System;

namespace ResourceManager.Commands
{
    public class RemoveCategoryRequest : IRequest<RemoveCategoryResults>
    {
        public RemoveCategoryRequest(Type resourceType, string category)
        {
            ResourceType = resourceType;
            Category = category;
        }

        public Type ResourceType { get; }
        public string Category { get; }
    }
}
