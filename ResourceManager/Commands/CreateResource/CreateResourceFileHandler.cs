using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class CreateResourceFileHandler<TResource> : CreateResourceHandler<TResource>
    {
        public override Task<CreateResourceResults> Handle(CreateResourceQuery<TResource> request, CancellationToken cancellationToken)
        {
            TResource resource = request.Resource;
            throw new System.NotImplementedException();
        }
    }
}
