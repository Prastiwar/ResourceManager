using ResourceManager.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager
{
    public static class ResourceDescriptorServiceExtensions
    {
        public static IEnumerable<IResourceDescriptor> Describe<T>(this IResourceDescriptorService creator) => creator.Describe(typeof(T));

        public static void Register<T>(this IResourceDescriptorService creator, params IResourceDescriptor[] descriptors)
            => creator.Register(typeof(T), RegisterOptions.Replace, descriptors);

        public static TDescriptor GetRequiredDescriptor<TDescriptor>(this IResourceDescriptorService service, Type type)
            where TDescriptor : IResourceDescriptor
        {
            IEnumerable<IResourceDescriptor> descriptors = service.Describe(type);
            IEnumerable<IResourceDescriptor> matchDescriptors = descriptors.Where(x => typeof(TDescriptor).IsAssignableFrom(x.GetType()));
            if (!matchDescriptors.Any())
            {
                throw new MissingDescriptorException($"Descriptor of type {typeof(TDescriptor)} was not found for resource: {type}");
            }
            TDescriptor exactDescriptor = (TDescriptor)matchDescriptors.FirstOrDefault(x => x.GetType() == typeof(TDescriptor));
            if (exactDescriptor == null)
            {
                return (TDescriptor)matchDescriptors.First();
            }
            return exactDescriptor;
        }
    }
}
