﻿using FluentFTP;
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

        private readonly IDictionary<Type, ResourcesEntry> cachedResources = new Dictionary<Type, ResourcesEntry>();

        public class ResourcesEntry
        {
            public IList<string> Files { get; set; }

            public IList<object> Resources { get; set; }

            public CachingPolicy CachingPolicy { get; set; }
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
            foreach (TrackingEntry tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.Resource).TrimStart('/', '\\'));
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
                            string originalPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.OriginalResource).TrimStart('/', '\\'));
                            if (!EqualityComparer<string>.Default.Equals(originalPath, targetPath))
                            {
                                client.DeleteFile(originalPath);
                            }
                        }
                        else if (updateStatus == FtpStatus.Failed)
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
            foreach (TrackingEntry tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.Resource).TrimStart('/', '\\'));
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
                        if (cachedResources.TryGetValue(tracking.ResourceType, out ResourcesEntry entry))
                        {
                            if (!entry.CachingPolicy.IsExpired)
                            {
                                entry.Resources.Add(tracking.Resource);
                                entry.Files.Add(targetPath);
                            }
                        }
                        break;
                    case ResourceState.Modified:
                        string updateContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        byte[] updateContentBytes = Encoding.UTF8.GetBytes(updateContent);
                        FtpStatus updateStatus = await client.UploadAsync(updateContentBytes, targetPath, FtpRemoteExists.Overwrite, true);
                        if (updateStatus == FtpStatus.Success)
                        {
                            string originalPath = Path.Combine(Options.RelativePath ?? "", descriptor.GetRelativeFullPath(tracking.OriginalResource).TrimStart('/', '\\'));
                            if (!EqualityComparer<string>.Default.Equals(originalPath, targetPath))
                            {
                                await client.DeleteFileAsync(originalPath);
                            }
                        }
                        else if (updateStatus == FtpStatus.Failed)
                        {
                            throw new FtpException("Update operation failed");
                        }
                        break;
                    case ResourceState.Removed:
                        await client.DeleteFileAsync(targetPath);
                        if (cachedResources.TryGetValue(tracking.ResourceType, out ResourcesEntry entryToRemove))
                        {
                            if (!entryToRemove.CachingPolicy.IsExpired)
                            {
                                entryToRemove.Resources.Remove(tracking.Resource);
                                entryToRemove.Files.Remove(targetPath);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        public override IQueryable<object> Query(Type resourceType)
        {
            if (cachedResources.TryGetValue(resourceType, out ResourcesEntry entry))
            {
                if (!entry.CachingPolicy.IsExpired)
                {
                    return entry.Resources.AsQueryable();
                }
            }
            entry = new ResourcesEntry() { 
                CachingPolicy = new CachingPolicy(Options.CacheExpirationTime)
            };
            FtpClient client = CreateClient();
            LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
            string path = Path.Combine(Options.RelativePath ?? "", descriptor.RelativeRootPath.TrimStart('/', '\\'));
            client.Connect();
            entry.Files = client.GetListing(path, FtpListOption.Recursive).Where(item => item.Type == FtpFileSystemObjectType.File).Select(item => item.FullName).ToList();
            entry.Resources = new List<object>(entry.Files.Count);
            foreach (string resourcePath in entry.Files)
            {
                if (client.Download(out byte[] bytes, resourcePath))
                {
                    string content = Encoding.UTF8.GetString(bytes);
                    object resource = Serializer.Deserialize(content, resourceType);
                    TrackingEntry newEntry = new TrackingEntry(resource, ResourceState.Unchanged, resourceType);
                    TrackingEntry trackedEntry = TrackedResources.FirstOrDefault(x => x.ResourceType == resourceType &&
                                                                                 x.State != ResourceState.Added &&
                                                                                 x.OriginalResource == resource);
                    if (trackedEntry == null)
                    {
                        TrackedResources.Add(newEntry);
                    }
                    entry.Resources.Add(resource);
                }
            }
            cachedResources[resourceType] = entry;
            return entry.Resources.AsQueryable();
        }
    }
}
