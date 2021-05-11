using ResourceManager.Services;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFtpHandler : GetResourceByPathFileHandler
    {
        public GetResourceByPathFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
