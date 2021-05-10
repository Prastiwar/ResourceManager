using MediatR;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByIdFileHandler : GetResourceByPathFileHandler, IRequestHandler<GetResourceByIdQuery, object>,
                                                                             IRequestHandler<GetResourcesByIdQuery, IEnumerable<object>>
    {
        public GetResourceByIdFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ISerializer serializer)
            : base(client, serializer) => DescriptorService = descriptorService;

        protected IResourceDescriptorService DescriptorService { get; }

        public async Task<object> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
        {
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
            IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
            foreach (string file in files)
            {
                KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(file);
                KeyValuePair<string, object> parameter = parameters.FirstOrDefault(x => string.Compare(x.Key, "id", true) == 0);
                if (parameter.Key != null && parameter.Value == request.Id)
                {
                    return await GetResourceByPath(request.ResourceType, file);
                }
            }
            return default;
        }

        public Task<IEnumerable<object>> Handle(GetResourcesByIdQuery request, CancellationToken cancellationToken)
        {
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
            string path = pathDescriptor.RelativeRootPath;
            return GetResourcesByPath(request.ResourceType, path);
        }
    }
}
