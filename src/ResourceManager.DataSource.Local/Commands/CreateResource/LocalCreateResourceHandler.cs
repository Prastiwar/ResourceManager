using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local.Commands
{
    public class LocalCreateResourceHandler : CreateResourceHandler
    {
        public override Task<CreateResourceResults> Handle(CreateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
