using Microsoft.Extensions.Configuration;
using ResourceManager.Data;
using ResourceManager.DataSource.Local.Configuration;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSource : DataSource
    {
        public LocalDataSource(IConfiguration configuration, IConnectionMonitor monitor, IResourceDescriptorService descriptorService, ITextSerializer serializer, LocalDataSourceOptions options)
            : base(configuration, monitor, descriptorService)
        {
            Serializer = serializer;
            Options = options;
        }

        public LocalDataSourceOptions Options { get; }

        protected ITextSerializer Serializer { get; }

        // WARNING: Saving changes is not consistent and not atomic
        // TODO: Fix consistency and atomicity
        public override void SaveChanges()
        {
            foreach (TrackingEntry tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.GetFullFolderPath(), descriptor.GetRelativeFullPath(tracking.Resource));
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        string addedContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        File.WriteAllText(targetPath, addedContent);
                        break;
                    case ResourceState.Modified:
                        string updateContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        File.WriteAllText(targetPath, updateContent);
                        string originalPath = Path.Combine(Options.GetFullFolderPath(), descriptor.GetRelativeFullPath(tracking.OriginalResource));
                        if (!EqualityComparer<string>.Default.Equals(originalPath, targetPath))
                        {
                            File.Delete(originalPath);
                        }
                        break;
                    case ResourceState.Removed:
                        File.Delete(targetPath);
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        // WARNING: Saving changes is not consistent and not atomic
        // TODO: Fix consistency and atomicity
        public override async Task SaveChangesAsync(CancellationToken token)
        {
            foreach (TrackingEntry tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.GetFullFolderPath(), descriptor.GetRelativeFullPath(tracking.Resource).TrimStart('/', '\\'));
                switch (tracking.State)
                {
                    case ResourceState.Added:
                        string addedContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        await File.WriteAllTextAsync(targetPath, addedContent);
                        break;
                    case ResourceState.Modified:
                        string updateContent = Serializer.Serialize(tracking.Resource, tracking.ResourceType);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        await File.WriteAllTextAsync(targetPath, updateContent);
                        string originalPath = Path.Combine(Options.GetFullFolderPath(), descriptor.GetRelativeFullPath(tracking.OriginalResource).TrimStart('/', '\\'));
                        if (!EqualityComparer<string>.Default.Equals(originalPath, targetPath))
                        {
                            File.Delete(originalPath);
                        }
                        break;
                    case ResourceState.Removed:
                        File.Delete(targetPath);
                        break;
                    default:
                        break;
                }
            }
            TrackedResources.Clear();
        }

        public override IQueryable<object> Query(Type resourceType)
        {
            LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
            string directoryPath = Path.Combine(Options.GetFullFolderPath(), descriptor.RelativeRootPath.TrimStart('/', '\\'));
            if (Directory.Exists(directoryPath))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directoryPath, Options.FileSearchPattern, SearchOption.AllDirectories).Select(path => path.Replace("\\", "/"));
                return files.Select(file => {
                    string content = File.ReadAllText(file);
                    object resource = Serializer.Deserialize(content, resourceType);
                    TrackingEntry newEntry = new TrackingEntry(resource, ResourceState.Unchanged, resourceType);
                    TrackedResources.Add(newEntry);
                    return resource;
                }).AsQueryable();
            }
            return Array.Empty<object>().AsQueryable();
        }
    }
}
