using ResourceManager.Services;

namespace ResourceManager.DataSource
{
    public interface IDataSourceProviderBuilderOptions
    {
        IResourceDescriptorService DescriptorService { get; }
    }
}
