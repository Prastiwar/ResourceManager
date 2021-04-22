using ResourceManager.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByIdFileHandler<TResource> : GetResourceByIdHandler<TResource>
    {
        public GetResourceByIdFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ISerializer serializer)
        {
            DescriptorService = descriptorService;
            Client = client;
            Serializer = serializer;
        }

        protected IResourceDescriptorService DescriptorService { get; }
        protected IFileClient Client { get; }
        protected ISerializer Serializer { get; }

        protected override async Task<TResource> GetResourceAsync(GetResourceByIdQuery<TResource> request, CancellationToken cancellationToken)
        {
            //IResourceDescriptor descriptor = DescriptorService.Create<TResource>();
            //if (descriptor is PathResourceDescriptor pathDescriptor)
            //{
            //  request.Id
            //}
            //throw new InvalidOperationException("Cannot retrieve resources which is not described by path descriptor");
            ; throw new NotImplementedException();
        }

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetResourceByIdQuery<TResource> request, CancellationToken cancellationToken)
        {
            IResourceDescriptor descriptor = DescriptorService.Create<TResource>();
            if (descriptor is PathResourceDescriptor pathDescriptor)
            {
                string path = pathDescriptor.RelativeRootPath;
                IEnumerable<string> files = await Client.ListFilesAsync(path);
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
            throw new InvalidOperationException("Cannot retrieve resources which is not described by path descriptor");
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
