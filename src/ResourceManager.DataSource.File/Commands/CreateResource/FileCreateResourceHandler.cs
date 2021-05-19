using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class FileCreateResourceHandler : CreateResourceHandler
    {
        public override Task<CreateResourceResults> Handle(CreateResourceRequest request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
