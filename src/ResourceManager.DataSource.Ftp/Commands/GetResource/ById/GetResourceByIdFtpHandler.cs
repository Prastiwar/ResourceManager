using ResourceManager.DataSource.Ftp.Data;
using ResourceManager.DataSource.Local.Commands;
using ResourceManager.DataSource.Local.Services;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class GetResourceByIdFtpHandler : GetResourceByIdFileHandler
    {
        public GetResourceByIdFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
