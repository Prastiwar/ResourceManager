using MediatR;

namespace ResourceManager.Commands
{
    public class DeleteResourceQuery<TResource> : IRequest<DeleteResourceResults>
    {
        public DeleteResourceQuery(TResource resource) => Resource = resource;

        public TResource Resource { get; }
    }
}
