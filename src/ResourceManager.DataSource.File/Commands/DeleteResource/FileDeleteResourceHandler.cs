using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class FileDeleteResourceHandler : DeleteResourceHandler
    {
        public FileDeleteResourceHandler(IFileClient client, IResourceDescriptorService descriptorService)
        {
            Client = client;
            DescriptorService = descriptorService;
        }

        protected IFileClient Client { get; }

        protected IResourceDescriptorService DescriptorService { get; }

        public override async Task<DeleteResourceResults> Handle(DeleteResourceRequest request, CancellationToken cancellationToken)
        {
            if (request.Resource == null)
            {
                return new DeleteResourceResults(false);
            }
            LocationResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(request.ResourceType);
            try
            {
                string path = pathDescriptor.GetRelativeFullPath(request.Resource);
                await Client.RemoveFileAsync(path);
            }
            catch (System.Exception ex)
            {
                return new DeleteResourceResults(false);
            }
            return new DeleteResourceResults(true);
        }
    }
}
