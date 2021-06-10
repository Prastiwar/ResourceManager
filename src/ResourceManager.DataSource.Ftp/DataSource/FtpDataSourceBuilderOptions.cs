using ResourceManager.Data;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSourceBuilderOptions
    {
        public string Name { get; set; }

        public ITextSerializer Serializer { get; set; }

        public IResourceDescriptorService DescriptorService { get; set; }
    }
}