using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class GetResourceByPathFileHandler : GetResourceFileHandler<GetResourceByPathQuery, GetResourcesByPathQuery>
    {
        public GetResourceByPathFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ITextSerializer serializer)
            : base(descriptorService, client, serializer) { }

        public override Task<object> Handle(GetResourceByPathQuery request, CancellationToken cancellationToken)
            => GetResourceByPath(request.ResourceType, request.Path);

        public override async Task<IEnumerable<object>> Handle(GetResourcesByPathQuery request, CancellationToken cancellationToken)
        {
            if (request.Paths == null || request.Paths.Length == 0)
            {
                PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
                IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
                return await GetResourceByPaths(request.ResourceType, files);
            }
            else
            {
                return await GetResourceByPaths(request.ResourceType, request.Paths);
            }
        }
    }
}
