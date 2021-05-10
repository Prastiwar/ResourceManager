using ResourceManager;
using ResourceManager.Services;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByPathFtpHandler<TResource> : GetPresentableByPathFileHandler<TResource>
    {
        public GetPresentableByPathFtpHandler(IFtpFileClient client, IResourceDescriptorService descriptorService) : base(client, descriptorService) { }
    }
}
