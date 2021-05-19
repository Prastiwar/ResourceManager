using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class CreateResourceHandler : IRequestHandler<CreateResourceRequest, CreateResourceResults>
    {
        public abstract Task<CreateResourceResults> Handle(CreateResourceRequest request, CancellationToken cancellationToken);
    }
}
