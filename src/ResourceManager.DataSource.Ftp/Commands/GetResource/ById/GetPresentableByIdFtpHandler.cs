using ResourceManager.Data;
using ResourceManager.DataSource.File.Commands;
using ResourceManager.DataSource.Ftp.Data;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class GetPresentableByIdFtpHandler : GetPresentableByIdFileHandler
    {
        public GetPresentableByIdFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ITextSerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
