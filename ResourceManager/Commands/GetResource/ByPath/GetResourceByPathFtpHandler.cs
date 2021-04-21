using ResourceManager.Services;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFtpHandler<TResource> : GetResourceByPathFileHandler<TResource>
    {
        public GetResourceByPathFtpHandler(IFtpFileClient client, ISerializer serializer) : base(client, serializer) { }
    }
}
