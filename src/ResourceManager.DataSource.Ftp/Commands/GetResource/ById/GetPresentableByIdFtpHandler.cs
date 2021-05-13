using ResourceManager.Services;
using ResourceManager.DataSource.Local.Commands;
using ResourceManager.DataSource.Local.Services;
using ResourceManager.DataSource.Ftp.Data;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class GetPresentableByIdFtpHandler : GetPresentableByIdFileHandler
    {
        public GetPresentableByIdFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
