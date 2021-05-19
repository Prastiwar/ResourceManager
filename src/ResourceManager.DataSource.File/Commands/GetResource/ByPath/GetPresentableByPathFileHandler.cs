using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class GetPresentableByPathFileHandler : GetPresentableHandler<GetPresentableByPathQuery, GetPresentablesByPathQuery>
    {
        public GetPresentableByPathFileHandler(IResourceDescriptorService descriptorService, IFileClient client)
            : base(descriptorService) => Client = client;

        protected IFileClient Client { get; }

        public override Task<PresentableData> Handle(GetPresentableByPathQuery request, CancellationToken cancellationToken)
            => GetPresentableByPath(request.ResourceType, request.Path);

        public override Task<IEnumerable<PresentableData>> Handle(GetPresentablesByPathQuery request, CancellationToken cancellationToken)
            => GetPresentablesByPath(request.ResourceType, request.Paths);

        protected async Task<IEnumerable<PresentableData>> GetPresentablesByPath(Type resourceType, string[] paths)
        {
            IList<PresentableData> presentables = new List<PresentableData>();
            List<Exception> exceptions = new List<Exception>();
            if (paths == null || paths.Length == 0)
            {
                PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(resourceType);
                IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
                await AddResourcesByPaths(resourceType, files, presentables, exceptions);
            }
            else
            {
                await AddResourcesByPaths(resourceType, paths, presentables, exceptions);
            }
            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more resources threw error while retrieving them", exceptions);
            }
            return presentables;
        }

        private async Task AddResourcesByPaths(Type resourceType, IEnumerable<string> paths, IList<PresentableData> resources, List<Exception> exceptions)
        {
            foreach (string path in paths)
            {
                try
                {
                    PresentableData resource = await GetPresentableByPath(resourceType, path);
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
    }
}
