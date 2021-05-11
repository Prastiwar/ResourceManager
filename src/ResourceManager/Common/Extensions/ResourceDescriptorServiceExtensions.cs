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

        public static void Register<T>(this ResourceDescriptorService creator, params IResourceDescriptor[] descriptors)
            => creator.Register(typeof(T), RegisterOptions.Replace, descriptors);

        public static TDescriptor GetRequiredDescriptor<TDescriptor>(this IResourceDescriptorService service, Type type)
            where TDescriptor : IResourceDescriptor
        {
            IEnumerable<IResourceDescriptor> descriptors = service.Describe(type);
            TDescriptor descriptor = descriptors.Where(x => x.GetType() == typeof(TDescriptor)).Cast<TDescriptor>().FirstOrDefault();
            if (descriptor == null)
            {
                throw new MissingDescriptorException($"Descriptor of type {typeof(TDescriptor)} was not found for resource: {type}");
            }
            return descriptor;
        }
    }
}
