using MediatR;

namespace ResourceManager.Commands
{
    public abstract class ResourceRequest<TResource, TResults> : IRequest<TResults>
    {
        public ResourceRequest(TResource resource) => Resource = resource;

        public TResource Resource { get; }
    }
}
