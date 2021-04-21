using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public abstract class RenameCategoryHandler<TResource> : IRequestHandler<RenameCategoryQuery<TResource>, RenameCategoryResults>
    {
        public async Task<RenameCategoryResults> Handle(RenameCategoryQuery<TResource> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Resource == null)
                {
                    await RenameCategoryAsync(request.OldCategory, request.NewCategory);
                }
                else
                {
                    await RenameCategoryAsync(request.Resource, request.NewCategory);
                }
            }
            catch (System.Exception)
            {
                return new RenameCategoryResults(false);
            }
            return new RenameCategoryResults(true);
        }

        protected abstract Task RenameCategoryAsync(string oldCategory, string newCategory);

        protected abstract Task RenameCategoryAsync(TResource resource, string newCategory);
    }
}
