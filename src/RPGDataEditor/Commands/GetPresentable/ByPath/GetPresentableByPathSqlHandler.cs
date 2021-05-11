using ResourceManager;
using ResourceManager.Data;
using ResourceManager.Services;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    /// <summary> Handles GetPresentableByPath where path should be formatted as [TableName].ResourceId </summary>
    public class GetPresentableByPathSqlHandler : GetPresentableHandler<GetPresentableByPathQuery, GetPresentablesByPathQuery>
    {
        public GetPresentableByPathSqlHandler(IResourceDescriptorService descriptorService, ISqlClient client)
            : base(descriptorService) => Client = client;

        protected ISqlClient Client { get; }

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
                SqlPathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<SqlPathResourceDescriptor>(resourceType);
                IEnumerable<object> resources = await Client.SelectAsync(pathDescriptor.TableName, resourceType);
                IList<string> resourcePaths = new List<string>();
                foreach (object resource in resources)
                {
                    resourcePaths.Add(pathDescriptor.GetRelativeFullPath(resource));
                }
                await AddResourcesByPaths(resourceType, resourcePaths, presentables, exceptions);
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
            SqlPathResourceDescriptor sqlPathDescriptor = DescriptorService.GetRequiredDescriptor<SqlPathResourceDescriptor>(resourceType);
            foreach (string path in paths)
            {
                try
                {
                    PresentableData resource = await GetPresentableByPath(resourceType, path, sqlPathDescriptor);
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
