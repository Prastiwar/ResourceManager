using MediatR;

namespace RPGDataEditor.Commands
{
    public class RemoveCategoryQuery<TResource> : IRequest<RemoveCategoryResults>
    {
        public RemoveCategoryQuery(string category) => Category = category;

        public string Category { get; }
    }
}
