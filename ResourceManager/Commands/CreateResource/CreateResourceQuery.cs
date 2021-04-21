namespace ResourceManager.Commands
{
    public class CreateResourceQuery<TResource> : ResourceQuery<TResource, CreateResourceResults>
    {
        public CreateResourceQuery(TResource resource) : base(resource) { }
    }
}
