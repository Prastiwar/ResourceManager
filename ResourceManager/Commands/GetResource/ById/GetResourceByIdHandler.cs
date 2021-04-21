namespace ResourceManager.Commands
{
    public abstract class GetResourceByIdHandler<TResource> : GetResourceHandler<GetResourceByIdQuery<TResource>, TResource> { }
}
