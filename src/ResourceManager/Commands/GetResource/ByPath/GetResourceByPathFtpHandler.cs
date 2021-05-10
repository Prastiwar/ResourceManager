using ResourceManager.Services;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFtpHandler : GetResourceByPathFileHandler
    {
        public GetResourceByPathFtpHandler(IFtpFileClient client, ISerializer serializer) : base(client, serializer) { }
    }
}
