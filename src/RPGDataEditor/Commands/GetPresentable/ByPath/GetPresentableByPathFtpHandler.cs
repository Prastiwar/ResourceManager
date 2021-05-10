using ResourceManager;
using ResourceManager.Services;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByPathFtpHandler : GetPresentableByPathFileHandler
    {
        public GetPresentableByPathFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client) : base(descriptorService, client) { }
    }
}
