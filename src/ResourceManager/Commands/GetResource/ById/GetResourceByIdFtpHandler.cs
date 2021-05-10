using ResourceManager.Services;

namespace ResourceManager.Commands
{
    public class GetResourceByIdFtpHandler : GetResourceByIdFileHandler
    {
        public GetResourceByIdFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
