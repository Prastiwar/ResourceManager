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
            foreach (ITrackedResource tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.FolderPath, descriptor.GetRelativeFullPath(tracking.Resource));
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
                        try
                        {
                            File.WriteAllText(targetPath, updateContent);
                            string oldPath = descriptor.GetRelativeFullPath(tracking.OriginalResource);
                            if (string.Compare(oldPath, targetPath) != 0)
                            {
                                File.Delete(oldPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
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
            foreach (ITrackedResource tracking in TrackedResources)
            {
                LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(tracking.ResourceType);
                string targetPath = Path.Combine(Options.FolderPath, descriptor.GetRelativeFullPath(tracking.Resource).TrimStart('/', '\\'));
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
                        try
                        {
                            await File.WriteAllTextAsync(targetPath, updateContent);
                            string oldPath = descriptor.GetRelativeFullPath(tracking.OriginalResource);
                            if (string.Compare(oldPath, targetPath) != 0)
                            {
                                File.Delete(oldPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
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
            => Locate(resourceType).ToArray().Select(path => {
            string content = File.ReadAllText(path);
            return Serializer.Deserialize(content, resourceType);
        }).AsQueryable();

        public override IQueryable<string> Locate(Type resourceType)
        {
            LocationResourceDescriptor descriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(resourceType);
            string directoryPath = Path.Combine(Options.FolderPath, descriptor.RelativeRootPath);
            IList<string> contents = new List<string>();
            if (Directory.Exists(directoryPath))
            {

                IEnumerable<string> files = Directory.EnumerateFiles(directoryPath, Options.FileSearchPattern, SearchOption.AllDirectories).Select(path => path.Replace("\\", "/"));
                foreach (string file in files)
                {
                    string content = File.ReadAllText(file);
                    contents.Add(content);
                }
            }
            return contents.AsQueryable();
        }
    }
}
