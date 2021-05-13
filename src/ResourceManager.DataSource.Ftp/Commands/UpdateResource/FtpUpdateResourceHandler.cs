using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class FtpUpdateResourceHandler : UpdateResourceHandler
    {
        public override Task<UpdateResourceResults> Handle(UpdateResourceQuery request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
