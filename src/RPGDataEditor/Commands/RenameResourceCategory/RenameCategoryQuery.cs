using ResourceManager.Commands;

namespace RPGDataEditor.Commands
{
    public class RenameCategoryQuery<TResource> : ResourceQuery<TResource, RenameCategoryResults>
    {
        public RenameCategoryQuery(TResource resource, string oldCategory, string newCategory) : base(resource)
        {
            OldCategory = oldCategory;
            NewCategory = newCategory;
        }

        public string OldCategory { get; }
        public string NewCategory { get; }
    }
}
