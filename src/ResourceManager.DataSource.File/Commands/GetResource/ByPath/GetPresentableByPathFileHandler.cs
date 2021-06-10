using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Linq;
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

        public override async Task<IEnumerable<PresentableData>> Handle(GetPresentablesByPathQuery request, CancellationToken cancellationToken)
        {
            if (request.Paths == null || request.Paths.Length == 0)
            {
                LocationResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(request.ResourceType);
                IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
                return await GetPresentableByPaths(request.ResourceType, files.ToArray());
            }
            else
            {
                return await GetPresentableByPaths(request.ResourceType, request.Paths);
            }
        }
    }
}
