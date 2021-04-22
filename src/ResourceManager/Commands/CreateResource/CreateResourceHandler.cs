using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class CreateResourceHandler<TResource> : IRequestHandler<CreateResourceQuery<TResource>, CreateResourceResults>
    {
        public abstract Task<CreateResourceResults> Handle(CreateResourceQuery<TResource> request, CancellationToken cancellationToken);
    }
}
