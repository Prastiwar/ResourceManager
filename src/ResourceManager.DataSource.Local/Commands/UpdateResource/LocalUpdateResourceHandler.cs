using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local.Commands
{
    public class LocalUpdateResourceHandler : UpdateResourceHandler
    {
        public override Task<UpdateResourceResults> Handle(UpdateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
