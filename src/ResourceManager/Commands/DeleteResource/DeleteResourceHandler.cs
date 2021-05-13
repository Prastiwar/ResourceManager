using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class DeleteResourceHandler : IRequestHandler<DeleteResourceQuery, DeleteResourceResults>
    {
        public abstract Task<DeleteResourceResults> Handle(DeleteResourceQuery request, CancellationToken cancellationToken);
    }
}
