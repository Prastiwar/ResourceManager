namespace ResourceManager.Commands
{
    public class CreateResourceQuery<TResource> : ResourceRequest<TResource, CreateResourceResults>
    {
        public CreateResourceQuery(TResource resource) : base(resource) { }
    }
}
