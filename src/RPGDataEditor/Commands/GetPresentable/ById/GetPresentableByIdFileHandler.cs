using MediatR;
using ResourceManager;
using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.Services;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByIdFileHandler : GetPresentableByPathFileHandler, IRequestHandler<GetPresentableByIdQuery, PresentableData>,
                                                                                  IRequestHandler<GetPresentableByIdQuery, IEnumerable<PresentableData>>
    {
        public GetPresentableByIdFileHandler(IFileClient client, IResourceDescriptorService descriptorService)
            : base(descriptorService, client) { }

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

        //protected override async Task<PresentableData> GetResourceAsync(GetPresentableByIdQuery<TResource> request, CancellationToken cancellationToken)
        //{
        //    IEnumerable<IResourceDescriptor> descriptors = DescriptorService.Describe<TResource>();
        //    PathResourceDescriptor pathDescriptor = descriptors.OfType<PathResourceDescriptor>().FirstOrDefault();
        //    if (pathDescriptor == null)
        //    {
        //        throw new InvalidOperationException("Cannot retrieve resources which is not described by path descriptor");
        //    }
        //    IEnumerable<string> files = await Client.ListFilesAsync(pathDescriptor.RelativeRootPath);
        //    foreach (string file in files)
        //    {
        //        KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(file);
        //        KeyValuePair<string, object> parameter = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id), true) == 0);
        //        if (parameter.Key != null && parameter.Value == request.Id)
        //        {
        //            return await GetPresentableByPath(file);
        //        }
        //    }
        //    return default;
        //}

        //protected override async Task ProcessResourcesAsync(IList<object> resources, GetPresentableByIdQuery<TResource> request, CancellationToken cancellationToken)
        //{
        //    IEnumerable<IResourceDescriptor> descriptors = DescriptorService.Describe<TResource>();
        //    PathResourceDescriptor pathDescriptor = descriptors.OfType<PathResourceDescriptor>().FirstOrDefault();
        //    if (pathDescriptor != null)
        //    {
        //        string path = pathDescriptor.RelativeRootPath;
        //        IEnumerable<string> files = await Client.ListFilesAsync(path);
        //        List<Exception> exceptions = new List<Exception>();
        //        foreach (string file in files)
        //        {
        //            try
        //            {
        //                PresentableData presentable = await GetPresentableByPath(file);
        //                if (!(presentable is null))
        //                {
        //                    resources.Add(presentable);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                exceptions.Add(ex);
        //            }
        //        }
        //        if (exceptions.Count > 0)
        //        {
        //            throw new AggregateException("One or more resources threw error while retrieving them", exceptions);
        //        }
        //    }
        //    throw new InvalidOperationException("Cannot retrieve resources which is not described by path descriptor");
        //}
    }
}
