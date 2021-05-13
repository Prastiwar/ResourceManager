using System;

namespace ResourceManager.Commands
{
    public class RenameCategoryQuery : ResourceRequest<RenameCategoryResults>
    {
        public RenameCategoryQuery(Type resourceType, object resource, string oldCategory, string newCategory) : base(resourceType, resource)
        {
            OldCategory = oldCategory;
            NewCategory = newCategory;
        }

        public string OldCategory { get; }
        public string NewCategory { get; }
    }
}
