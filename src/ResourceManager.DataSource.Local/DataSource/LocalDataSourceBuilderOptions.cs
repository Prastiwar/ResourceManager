using ResourceManager.Data;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSourceBuilderOptions : IDataSourceProviderBuilderOptions
    {
        public string Name { get; set; }

        public string FileSearchPattern { get; set; }

        public string FolderPath { get; set; }

        public ITextSerializer Serializer { get; set; }

        public IResourceDescriptorService DescriptorService { get; set; }
    }
}