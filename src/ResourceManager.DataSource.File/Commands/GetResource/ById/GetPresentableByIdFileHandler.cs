﻿using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.File.Commands
{
    public class GetPresentableByIdFileHandler : GetPresentableHandler<GetPresentableByIdQuery, GetPresentablesByIdQuery>
    {
        public GetPresentableByIdFileHandler(IResourceDescriptorService descriptorService, IFileClient client)
            : base(descriptorService) => Client = client;

        protected IFileClient Client { get; }

        public override async Task<PresentableData> Handle(GetPresentableByIdQuery request, CancellationToken cancellationToken)
        {
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
            IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
            foreach (string file in files)
            {
                KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(file);
                KeyValuePair<string, object> parameter = parameters.FirstOrDefault(x => string.Compare(x.Key, "id", true) == 0);
                if (parameter.Key != null && parameter.Value == request.Id)
                {
                    return await GetPresentableByPath(request.ResourceType, file);
                }
            }
            return default;
        }

        public override async Task<IEnumerable<PresentableData>> Handle(GetPresentablesByIdQuery request, CancellationToken cancellationToken)
        {
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(request.ResourceType);
            IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
            IList<string> paths = new List<string>();
            if (request.Ids != null || request.Ids.Length > 0)
            {
                foreach (string file in files)
                {
                    KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(file);
                    KeyValuePair<string, object> idParameter = parameters.FirstOrDefault(x => string.Compare(x.Key, "id", true) == 0);
                    if (idParameter.Key != null && request.Ids.Any(id => id == idParameter.Value))
                    {
                        paths.Add(file);
                    }
                }
            }
            return await GetPresentableByPaths(request.ResourceType, paths);
        }
    }
}
