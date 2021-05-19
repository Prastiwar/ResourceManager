using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class FileDeleteResourceHandler : DeleteResourceHandler
    {
        public override Task<DeleteResourceResults> Handle(DeleteResourceRequest request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
