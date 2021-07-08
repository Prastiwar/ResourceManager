using ResourceManager.Data;
using ResourceManager.Services;
using System;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSourceBuilderOptions : IDataSourceProviderBuilderOptions
    {
        public string Name { get; set; }

        public ITextSerializer Serializer { get; set; }

        public IResourceDescriptorService DescriptorService { get; set; }

        public TimeSpan CacheExpirationTime { get; set; }
    }
}