using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class FileUpdateResourceHandler : UpdateResourceHandler
    {
        public override Task<UpdateResourceResults> Handle(UpdateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
