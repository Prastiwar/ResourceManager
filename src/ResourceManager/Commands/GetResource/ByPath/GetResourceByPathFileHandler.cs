using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByPathFileHandler : ResourceRequestHandler<GetResourceByPathQuery, GetResourcesByPathQuery>
    {
        public GetResourceByPathFileHandler(IFileClient client, ISerializer serializer)
        {
            Client = client;
            Serializer = serializer;
        }

        protected IFileClient Client { get; }

        protected ISerializer Serializer { get; }

        public override Task<object> Handle(GetResourceByPathQuery request, CancellationToken cancellationToken)
            => GetResourceByPath(request.ResourceType, request.Path);

        public override Task<IEnumerable<object>> Handle(GetResourcesByPathQuery request, CancellationToken cancellationToken)
            => GetResourcesByPath(request.ResourceType, request.Path);

        protected async Task<IEnumerable<object>> GetResourcesByPath(Type resourceType, string path)
        {
            IEnumerable<string> files = await Client.ListFilesAsync(path);
            List<Exception> exceptions = new List<Exception>();
            IList<object> resources = new List<object>();
            foreach (string file in files)
            {
                try
                {
                    object resource = await GetResourceByPath(resourceType, file);
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
            return resources;
        }

        protected async Task<object> GetResourceByPath(Type resourceType, string path)
        {
            string content = await Client.ReadFileAsync(path);
            object resource = Serializer.Deserialize(content, resourceType);
            return resource;
        }
    }
}
