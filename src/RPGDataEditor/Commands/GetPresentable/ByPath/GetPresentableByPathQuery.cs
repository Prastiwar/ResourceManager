using MediatR;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByPathQuery<TResource> : IRequest<TResource>
    {
        public GetPresentableByPathQuery(string path) => Path = path;

        public string Path { get; }
    }
}
