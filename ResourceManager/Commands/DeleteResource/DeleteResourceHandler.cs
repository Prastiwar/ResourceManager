using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class DeleteResourceHandler<TResource> : IRequestHandler<DeleteResourceQuery<TResource>, DeleteResourceResults>
    {
        public abstract Task<DeleteResourceResults> Handle(DeleteResourceQuery<TResource> request, CancellationToken cancellationToken);
    }
}
