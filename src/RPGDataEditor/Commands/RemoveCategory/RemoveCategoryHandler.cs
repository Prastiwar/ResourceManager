using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public abstract class RemoveCategoryHandler<TResource> : IRequestHandler<RemoveCategoryQuery<TResource>, RemoveCategoryResults>
    {
        public async Task<RemoveCategoryResults> Handle(RemoveCategoryQuery<TResource> request, CancellationToken cancellationToken)
        {
            try
            {
                await RemoveCategoryAsync(request.Category);
            }
            catch (System.Exception)
            {
                return new RemoveCategoryResults(false);
            }
            return new RemoveCategoryResults(true);
        }

        protected abstract Task RemoveCategoryAsync(string category);
    }
}
