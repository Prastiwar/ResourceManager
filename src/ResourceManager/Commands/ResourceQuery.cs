using MediatR;

namespace ResourceManager.Commands
{
    public class ResourceQuery<TResource, TResults> : IRequest<TResults>
    {
        public ResourceQuery(TResource resource) => Resource = resource;

        public TResource Resource { get; }
    }
}
