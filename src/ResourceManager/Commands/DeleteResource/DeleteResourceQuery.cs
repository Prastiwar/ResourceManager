using MediatR;

namespace ResourceManager.Commands
{
    public sealed class DeleteResourceQuery<TResource> : IRequest<DeleteResourceResults>
    {
        public DeleteResourceQuery(TResource resource) => Resource = resource;

        public TResource Resource { get; }
    }
}
