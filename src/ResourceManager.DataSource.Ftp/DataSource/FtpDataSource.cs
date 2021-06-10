using FluentFTP;
using Microsoft.Extensions.Configuration;
using ResourceManager.Data;
using ResourceManager.DataSource.Ftp.Configuration;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpDataSource : DataSource
    {
        public FtpDataSource(IConfiguration configuration, IConnectionMonitor monitor, ITextSerializer serializer, IResourceDescriptorService descriptorService, FtpDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            Serializer = serializer;
            DescriptorService = descriptorService;
            Options = options;
        }

        public FtpDataSourceOptions Options { get; }

        public class ResourcesEntry
        {
            public IList<FtpListItem> Files { get; set; }

            public IList<object> Resources { get; set; }

            //public CachingPolicy CachingPolicy { get; set; }
        }

        protected ITextSerializer Serializer { get; }
        protected IResourceDescriptorService DescriptorService { get; }

        private readonly IDictionary<Type, ResourcesEntry> entries = new Dictionary<Type, ResourcesEntry>();

        public override void SaveChanges()
        {
            throw new NotImplementedException();
            foreach (ITrackedResource tracking in TrackedResources)
            {
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        break;
                    case ResourceState.Modified:
                        break;
                    case ResourceState.Removed:
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        public override async Task SaveChangesAsync(CancellationToken token)
        {
            throw new NotImplementedException();
            foreach (ITrackedResource tracking in TrackedResources)
            {
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        break;
                    case ResourceState.Modified:
                        break;
                    case ResourceState.Removed:
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        public override IQueryable<object> Query(Type resourceType)
        {
            if (entries.TryGetValue(resourceType, out ResourcesEntry entry))
            {
                //if (!entry.CachingPolicy.IsExpired())
                //{
                //    return entry.Resources.AsQueryable();
                //}
                return entry.Resources.AsQueryable();
            }
            entry = new ResourcesEntry();
            FtpClient client = CreateClient();
            PathResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(resourceType);
            string path = Path.Combine(Options.RelativePath ?? "", descriptor.RelativeRootPath);
            client.Connect();
            entry.Files = client.GetListing(path, FtpListOption.Recursive).Where(item => item.Type == FtpFileSystemObjectType.File).ToList();
            entry.Resources = new List<object>(entry.Files.Count);
            foreach (string resourcePath in entry.Files.Select(x => x.FullName))
            {
                if (client.Download(out byte[] bytes, resourcePath))
                {
                    string content = Encoding.UTF8.GetString(bytes);
                    object resource = Serializer.Deserialize(content, resourceType);
                    entry.Resources.Add(resource);
                }
            }
            entries[resourceType] = entry;
            return entry.Resources.AsQueryable();
        }

        private FtpClient CreateClient() => new FtpClient() {
            Host = Options.Host,
            Port = Options.Port,
            Credentials = new System.Net.NetworkCredential(Options.UserName, Options.Password)
        };

        public override IQueryable<string> Locate(Type resourceType)
        {
            FtpClient client = CreateClient();
            PathResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(resourceType);
            string path = Path.Combine(Options.RelativePath ?? "", descriptor.RelativeRootPath);
            client.Connect();
            return client.GetListing(path, FtpListOption.Recursive)
                         .Where(item => item.Type == FtpFileSystemObjectType.File)
                         .Select(item => item.FullName)
                         .AsQueryable();
        }
    }
}
