namespace ResourceManager.Commands
{
    public class UpdateResourceQuery<TResource> : ResourceRequest<TResource, UpdateResourceResults>
    {
        public UpdateResourceQuery(TResource oldResource, TResource resource) : base(resource) => OldResource = oldResource;

        public TResource OldResource { get; }
    }
}
