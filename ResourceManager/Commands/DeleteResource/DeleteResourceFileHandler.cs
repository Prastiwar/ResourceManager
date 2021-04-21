using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class DeleteResourceFileHandler<TResource> : DeleteResourceHandler<TResource>
    {
        public override Task<DeleteResourceResults> Handle(DeleteResourceQuery<TResource> request, CancellationToken cancellationToken)
        {
            TResource resource = request.Resource;
            throw new System.NotImplementedException();
        }
    }
}
