using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class UpdateResourceHandler<TResource> : IRequestHandler<UpdateResourceQuery<TResource>, UpdateResourceResults>
    {
        public abstract Task<UpdateResourceResults> Handle(UpdateResourceQuery<TResource> request, CancellationToken cancellationToken);
    }
}
