namespace ResourceManager.Commands
{
    public sealed class CreateResourceQuery<TResource> : ResourceRequest<TResource, CreateResourceResults>
    {
        public CreateResourceQuery(TResource resource) : base(resource) { }
    }
}
