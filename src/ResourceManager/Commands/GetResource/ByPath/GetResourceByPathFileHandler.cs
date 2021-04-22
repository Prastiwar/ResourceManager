using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFileHandler<TResource> : GetResourceByPathHandler<TResource>
    {
        public GetResourceByPathFileHandler(IFileClient client, ISerializer serializer)
        {
            Client = client;
            Serializer = serializer;
        }

        protected IFileClient Client { get; }

        protected ISerializer Serializer { get; }

        protected override async Task<TResource> GetResourceAsync(GetResourceByPathQuery<TResource> request, CancellationToken cancellationToken) => (TResource)await GetResourceByPath(request.Path);

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetResourceByPathQuery<TResource> request, CancellationToken cancellationToken)
        {
            IEnumerable<string> files = await Client.ListFilesAsync(request.Path);
            List<Exception> exceptions = new List<Exception>();
            foreach (string file in files)
            {
                try
                {
                    object resource = await GetResourceByPath(file);
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
            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more resources threw error while retrieving them", exceptions);
            }
        }

        protected async Task<object> GetResourceByPath(string path)
        {
            string content = await Client.ReadFileAsync(path);
            Type elementType = typeof(TResource);
            object resource = Serializer.Deserialize(content, elementType);
            return resource;
        }
    }
}
