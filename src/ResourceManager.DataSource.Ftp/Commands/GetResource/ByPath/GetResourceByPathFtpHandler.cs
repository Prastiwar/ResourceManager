using ResourceManager.DataSource.Ftp.Data;
using ResourceManager.DataSource.Local.Commands;
using ResourceManager.DataSource.Local.Services;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class GetResourceByPathFtpHandler : GetResourceByPathFileHandler
    {
        public GetResourceByPathFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
