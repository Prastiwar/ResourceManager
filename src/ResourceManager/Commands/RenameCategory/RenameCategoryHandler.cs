using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class RenameCategoryHandler : IRequestHandler<RenameCategoryQuery, RenameCategoryResults>
    {
        public async Task<RenameCategoryResults> Handle(RenameCategoryQuery request, CancellationToken cancellationToken)
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

        protected abstract Task RenameCategoryAsync(object resource, string newCategory);
    }
}
