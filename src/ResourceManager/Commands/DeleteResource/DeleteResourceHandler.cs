using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class DeleteResourceHandler : IRequestHandler<DeleteResourceRequest, DeleteResourceResults>
    {
        public abstract Task<DeleteResourceResults> Handle(DeleteResourceRequest request, CancellationToken cancellationToken);
    }
}
