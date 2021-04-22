using MediatR;

namespace ResourceManager.Commands
{
    public class GetResourceByIdQuery<TResource> : IRequest<TResource>
    {
        public GetResourceByIdQuery(object id = null) => Id = id;

        public object Id { get; }
    }
}
