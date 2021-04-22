namespace ResourceManager.Commands
{
    public abstract class GetResourceByPathHandler<TResource> : GetResourceHandler<GetResourceByPathQuery<TResource>, TResource> { }
}
