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
        public FtpDataSource(IConfiguration configuration, IConnectionMonitor monitor, IResourceDescriptorService descriptorService, ITextSerializer serializer, FtpDataSourceOptions options)
            : base(configuration, monitor, descriptorService)
        {
            Serializer = serializer;
            Options = options;
        }

        public FtpDataSourceOptions Options { get; }

        protected ITextSerializer Serializer { get; }

        private readonly IDictionary<Type, ResourcesEntry> entries = new Dictionary<Type, ResourcesEntry>();

        public class ResourcesEntry
        {
            public IList<FtpListItem> Files { get; set; }

            public IList<object> Resources { get; set; }

            //public CachingPolicy CachingPolicy { get; set; }
        }

        protected virtual FtpClient CreateClient() => new FtpClient() {
            Host = Options.Host,
            Port = Options.Port,
            Credentials = new System.Net.NetworkCredential(Options.UserName, Options.Password)
        };

        // TODO: Fix consistency and atomicity
        // WARNING: Saving changes is not consistent and not atomic
        public override void SaveChanges()
        {
            FtpClient client = CreateClient();
            client.Connect();
            foreach (ITrackedResource tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.Resource));
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        string addedContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        byte[] addedContentBytes = Encoding.UTF8.GetBytes(addedContent);
                        FtpStatus createStatus = client.Upload(addedContentBytes, targetPath, FtpRemoteExists.Overwrite, true);
                        if (createStatus == FtpStatus.Failed)
                        {
                            throw new FtpException("Add operation failed");
                        }
                        break;
                    case ResourceState.Modified:
                        string updateContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        byte[] updateContentBytes = Encoding.UTF8.GetBytes(updateContent);
                        FtpStatus updateStatus = client.Upload(updateContentBytes, targetPath, FtpRemoteExists.Overwrite, true);
                        if (updateStatus == FtpStatus.Success)
                        {
                            string oldPath = descriptor.GetRelativeFullPath(tracking.OriginalResource);
                            if (string.Compare(oldPath, targetPath) != 0)
                            {
                                client.DeleteFile(oldPath);
                            }
                        } else if (updateStatus == FtpStatus.Failed)
                        {
                            throw new FtpException("Update operation failed");
                        }
                        break;
                    case ResourceState.Removed:
                        client.DeleteFile(targetPath);
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        // TODO: Fix consistency and atomicity
        // WARNING: Saving changes is not consistent and not atomic
        public override async Task SaveChangesAsync(CancellationToken token)
        {
            FtpClient client = CreateClient();
            await client.ConnectAsync();
            foreach (ITrackedResource tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.Resource));
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        string addedContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        byte[] addedContentBytes = Encoding.UTF8.GetBytes(addedContent);
                        FtpStatus createStatus = await client.UploadAsync(addedContentBytes, targetPath, FtpRemoteExists.Overwrite, true);
                        if (createStatus == FtpStatus.Failed)
                        {
                            throw new FtpException("Add operation failed");
                        }
                        break;
                    case ResourceState.Modified:
                        string updateContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        byte[] updateContentBytes = Encoding.UTF8.GetBytes(updateContent);
                        FtpStatus updateStatus = await client.UploadAsync(updateContentBytes, targetPath, FtpRemoteExists.Overwrite, true);
                        if (updateStatus == FtpStatus.Success)
                        {
                            string oldPath = descriptor.GetRelativeFullPath(tracking.OriginalResource);
                            if (string.Compare(oldPath, targetPath) != 0)
                            {
                                await client.DeleteFileAsync(oldPath);
                            }
                        }
                        else if (updateStatus == FtpStatus.Failed)
                        {
                            throw new FtpException("Update operation failed");
                        }
                        break;
                    case ResourceState.Removed:
                        await client.DeleteFileAsync(targetPath);
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        public override IQueryable<object> Query(Type resourceType)
        {
            // TODO: Optimize it with cache
            ResourcesEntry entry = null;
            //if (entries.TryGetValue(resourceType, out ResourcesEntry entry))
            //{
            //    if (!entry.CachingPolicy.IsExpired())
            //    {
            //        return entry.Resources.AsQueryable();
            //    }
            //    return entry.Resources.AsQueryable();
            //}
            entry = new ResourcesEntry();
            FtpClient client = CreateClient();
            LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
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

        public override IQueryable<string> Locate(Type resourceType)
        {
            FtpClient client = CreateClient();
            LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
            string path = Path.Combine(Options.RelativePath ?? "", descriptor.RelativeRootPath);
            client.Connect();
            return client.GetListing(path, FtpListOption.Recursive)
                         .Where(item => item.Type == FtpFileSystemObjectType.File)
                         .Select(item => item.FullName)
                         .AsQueryable();
        }
    }
}
