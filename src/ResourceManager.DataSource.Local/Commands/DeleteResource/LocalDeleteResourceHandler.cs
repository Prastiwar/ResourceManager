using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local.Commands
{
    public class LocalDeleteResourceHandler : DeleteResourceHandler
    {
        public override Task<DeleteResourceResults> Handle(DeleteResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
