using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public abstract class GetResourceFileHandler<TQuery, TEnumerableQuery> : ResourceRequestHandler<TQuery, TEnumerableQuery>
        where TQuery : IResourceQuery
        where TEnumerableQuery : IEnumerableResourceQuery
    {
        public GetResourceFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ITextSerializer serializer)
        {
            DescriptorService = descriptorService;
            Client = client;
            Serializer = serializer;
        }

        protected IFileClient Client { get; }

        protected ITextSerializer Serializer { get; }

        protected IResourceDescriptorService DescriptorService { get; }

        protected async Task<IEnumerable<object>> GetResourceByPaths(Type resourceType, IEnumerable<string> paths)
        {
            if (paths == null)
            {
                return Array.Empty<object>();
            }
            IList<object> presentables = new List<object>();
            foreach (string path in paths)
            {
                object data = await GetResourceByPath(resourceType, path);
                presentables.Add(data);
            }
            return presentables;
        }

        protected async Task<object> GetResourceByPath(Type resourceType, string path)
        {
            string content = await Client.ReadFileAsync(path);
            object resource = Serializer.Deserialize(content, resourceType);
            return resource;
        }
    }
}
