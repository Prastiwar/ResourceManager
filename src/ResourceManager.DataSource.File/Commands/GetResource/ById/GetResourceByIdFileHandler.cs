using MediatR;
using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class GetResourceByIdFileHandler : GetResourceByPathFileHandler, IRequestHandler<GetResourceByIdQuery, object>,
                                                                             IRequestHandler<GetResourcesByIdQuery, IEnumerable<object>>
    {
        public GetResourceByIdFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ITextSerializer serializer)
            : base(descriptorService, client, serializer) { }

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

        public async Task<IEnumerable<object>> Handle(GetResourcesByIdQuery request, CancellationToken cancellationToken)
        {
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
            IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
            IList<string> paths = new List<string>();
            if (request.Ids != null && request.Ids.Length > 0)
            {
                foreach (string file in files)
                {
                    KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(file);
                    KeyValuePair<string, object> parameter = parameters.FirstOrDefault(x => string.Compare(x.Key, "id", true) == 0);
                    if (parameter.Key != null && request.Ids.Any(id => id == parameter.Value))
                    {
                        paths.Add(file);
                    }
                }
            }
            return await GetResourcesByPath(request.ResourceType, paths.ToArray());
        }
    }
}
