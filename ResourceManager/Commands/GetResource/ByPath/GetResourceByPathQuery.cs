using MediatR;

namespace ResourceManager.Commands
{
    public class GetResourceByPathQuery<TResource> : IRequest<TResource>
    {
        public GetResourceByPathQuery(string path) => Path = path;

        public string Path { get; }
    }
}
