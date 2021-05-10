using ResourceManager;
using ResourceManager.Services;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByPathFileHandler<TResource> : GetPresentableByPathHandler<TResource>
    {
        public GetPresentableByPathFileHandler(IFileClient client, IResourceDescriptorService descriptorService)
            : base(descriptorService) => Client = client;

        protected IFileClient Client { get; }

        protected override async Task<PresentableData> GetResourceAsync(GetPresentableByPathQuery<TResource> request, CancellationToken cancellationToken)
            => await GetPresentableByPath(request.Path);

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetPresentableByPathQuery<TResource> request, CancellationToken cancellationToken)
        {
            IEnumerable<string> files = await Client.ListFilesAsync(request.Path);
            List<Exception> exceptions = new List<Exception>();
            foreach (string file in files)
            {
                try
                {
                    PresentableData resource = await GetPresentableByPath(file);
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
    }
}
