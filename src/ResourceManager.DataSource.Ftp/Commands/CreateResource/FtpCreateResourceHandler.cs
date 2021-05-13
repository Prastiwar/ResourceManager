using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class FtpCreateResourceHandler : CreateResourceHandler
    {
        public override Task<CreateResourceResults> Handle(CreateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
