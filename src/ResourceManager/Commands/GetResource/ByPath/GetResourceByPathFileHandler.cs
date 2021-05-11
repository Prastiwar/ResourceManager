using ResourceManager.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFileHandler : ResourceRequestHandler<GetResourceByPathQuery, GetResourcesByPathQuery>
    {
        public GetResourceByPathFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ISerializer serializer)
        {
            DescriptorService = descriptorService;
            Client = client;
            Serializer = serializer;
        }

        protected IFileClient Client { get; }

        protected ISerializer Serializer { get; }

        protected IResourceDescriptorService DescriptorService { get; }

        public override Task<object> Handle(GetResourceByPathQuery request, CancellationToken cancellationToken)
            => GetResourceByPath(request.ResourceType, request.Path);

        public override Task<IEnumerable<object>> Handle(GetResourcesByPathQuery request, CancellationToken cancellationToken)
            => GetResourcesByPath(request.ResourceType, request.Paths);

        protected async Task<IEnumerable<object>> GetResourcesByPath(Type resourceType, string[] paths)
        {
            IList<object> resources = new List<object>();
            List<Exception> exceptions = new List<Exception>();
            if (paths == null || paths.Length == 0)
            {
                PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(resourceType);
                IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
                await AddResourcesByPaths(resourceType, files, resources, exceptions);
            }
            else
            {
                await AddResourcesByPaths(resourceType, paths, resources, exceptions);
            }
            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more resources threw error while retrieving them", exceptions);
            }
            return resources;
        }

        private async Task AddResourcesByPaths(Type resourceType, IEnumerable<string> paths, IList<object> resources, List<Exception> exceptions)
        {
            foreach (string path in paths)
            {
                try
                {
                    object resource = await GetResourceByPath(resourceType, path);
                    if (!(resource is null))
                    {
                        resources.Add(resource);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
        }

        protected async Task<object> GetResourceByPath(Type resourceType, string path)
        {
            string content = await Client.ReadFileAsync(path);
            object resource = Serializer.Deserialize(content, resourceType);
            return resource;
        }
    }
}
