﻿using MediatR;
using ResourceManager;
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
    public abstract class GetPresentableHandler<TQuery, TEnumerableQuery> : IRequestHandler<TQuery, PresentableData>,
                                                                            IRequestHandler<TEnumerableQuery, IEnumerable<PresentableData>>
        where TQuery : IRequest<PresentableData>
        where TEnumerableQuery : IRequest<IEnumerable<PresentableData>>
    {
        protected GetPresentableHandler(IResourceDescriptorService descriptorService) => DescriptorService = descriptorService;

        protected IResourceDescriptorService DescriptorService { get; }

        public abstract Task<PresentableData> Handle(TQuery request, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<PresentableData>> Handle(TEnumerableQuery request, CancellationToken cancellationToken);

        protected Task<PresentableData> GetPresentableByPath(Type resourceType, string path, PathResourceDescriptor pathDescriptor = null)
        {
            if (pathDescriptor == null)
            {
                pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(resourceType);
            }
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(path);
            PresentableData data = null;
            KeyValuePair<string, object> categoryParameter = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableCategoryData.Category)) == 0);
            if (categoryParameter.Key != null)
            {
                data = new PresentableCategoryData(resourceType) {
                    Category = categoryParameter.Value.ToString(),
                };
            }
            else
            {
                data = new PresentableData(resourceType);
            }

            data.Id = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id)) == 0).Value;
            data.Name = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id)) == 0).Value?.ToString();
            return Task.FromResult(data);
        }
    }
}
