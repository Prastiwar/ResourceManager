using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class RemoveCategoryHandler : IRequestHandler<RemoveCategoryQuery, RemoveCategoryResults>
    {
        public async Task<RemoveCategoryResults> Handle(RemoveCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await RemoveCategoryAsync(request.ResourceType, request.Category);
            }
            catch (Exception)
            {
                return new RemoveCategoryResults(false);
            }
            return new RemoveCategoryResults(true);
        }

        protected abstract Task RemoveCategoryAsync(Type resourceType, string category);
    }
}
