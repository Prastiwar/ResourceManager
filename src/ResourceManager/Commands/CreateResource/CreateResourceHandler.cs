using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class CreateResourceHandler : IRequestHandler<CreateResourceQuery, CreateResourceResults>
    {
        public abstract Task<CreateResourceResults> Handle(CreateResourceQuery request, CancellationToken cancellationToken);
    }
}
