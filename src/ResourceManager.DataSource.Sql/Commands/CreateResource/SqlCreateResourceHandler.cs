using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql.Commands
{
    public class SqlCreateResourceHandler : CreateResourceHandler
    {
        public override Task<CreateResourceResults> Handle(CreateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
