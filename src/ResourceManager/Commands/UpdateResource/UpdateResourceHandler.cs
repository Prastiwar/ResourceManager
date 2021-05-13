using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class UpdateResourceHandler : IRequestHandler<UpdateResourceQuery, UpdateResourceResults>
    {
        public abstract Task<UpdateResourceResults> Handle(UpdateResourceQuery request, CancellationToken cancellationToken);
    }
}
