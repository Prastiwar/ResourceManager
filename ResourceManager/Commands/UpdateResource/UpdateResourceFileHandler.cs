using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class UpdateResourceFileHandler<TResource> : UpdateResourceHandler<TResource>
    {
        public override Task<UpdateResourceResults> Handle(UpdateResourceQuery<TResource> request, CancellationToken cancellationToken)
        {
            TResource resource = request.Resource;
            throw new System.NotImplementedException();
        }
    }
}
