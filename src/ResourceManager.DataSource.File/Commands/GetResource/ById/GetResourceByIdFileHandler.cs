using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class GetResourceByIdFileHandler : GetResourceFileHandler<GetResourceByIdQuery, GetResourcesByIdQuery>
    {
        public GetResourceByIdFileHandler(IResourceDescriptorService descriptorService, IFileClient client, ITextSerializer serializer)
            : base(descriptorService, client, serializer) { }

        public override async Task<object> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
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

        public override async Task<IEnumerable<object>> Handle(GetResourcesByIdQuery request, CancellationToken cancellationToken)
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
            return await GetResourceByPaths(request.ResourceType, paths);
        }
    }
}
