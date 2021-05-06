using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public abstract class GetResourceHandler<TRequest, TResource> : IRequestHandler<TRequest, TResource> where TRequest : IRequest<TResource>
    {
        public async Task<TResource> Handle(TRequest request, CancellationToken cancellationToken)
        {
            bool isEnumerable = ShouldProcessMultipleResources();
            if (isEnumerable)
            {
                List<object> resources = new List<object>();
                await ProcessResourcesAsync(resources, request, cancellationToken);
                return ResolveEnumerableToResource(resources.AsEnumerable());
            }
            return await GetResourceAsync(request, cancellationToken);
        }

        protected abstract Task<TResource> GetResourceAsync(TRequest request, CancellationToken cancellationToken);

        protected abstract Task ProcessResourcesAsync(IList<object> resources, TRequest request, CancellationToken cancellationToken);

        protected virtual bool ShouldProcessMultipleResources() => typeof(IEnumerable).IsAssignableFrom(typeof(TResource));

        protected virtual TResource ResolveEnumerableToResource(IEnumerable<object> enumerable)
        {
            Type resourceType = typeof(TResource);
            if (resourceType.IsArray)
            {
                Type arrayElementType = typeof(TResource).GetElementType();
                return (TResource)(object)Array.CreateInstance(arrayElementType, enumerable.Count());
            }
            Type elementType = resourceType.GetGenericArguments().FirstOrDefault();
            if (elementType == null)
            {
                throw new NotSupportedException("Cannot resolve non-generic IEnumerable type " + resourceType);
            }
            Type listType = typeof(List<>).MakeGenericType(elementType);
            if (!resourceType.IsAssignableFrom(listType))
            {
                throw new NotSupportedException("Cannot resolve IEnumerable type " + resourceType);
            }
            return (TResource)Activator.CreateInstance(listType);
        }
    }
}
