using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class FtpDeleteResourceHandler : DeleteResourceHandler
    {
        public override Task<DeleteResourceResults> Handle(DeleteResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
