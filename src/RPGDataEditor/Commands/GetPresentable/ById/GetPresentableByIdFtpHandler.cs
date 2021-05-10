using ResourceManager;
using ResourceManager.Services;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByIdFtpHandler : GetPresentableByIdFileHandler
    {
        public GetPresentableByIdFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client, ISerializer serializer)
            : base(descriptorService, client, serializer) { }
    }
}
