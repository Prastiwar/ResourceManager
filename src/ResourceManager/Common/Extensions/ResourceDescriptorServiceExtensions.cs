using ResourceManager.Data;
using ResourceManager.Services;
using System.Collections.Generic;

namespace ResourceManager
{
    public static class ResourceDescriptorServiceExtensions
    {
        public static IEnumerable<IResourceDescriptor> Describe<T>(this IResourceDescriptorService creator) => creator.Describe(typeof(T));
        public static void Register<T>(this ResourceDescriptorService creator, params IResourceDescriptor[] descriptors) => creator.Register(typeof(T), RegisterOptions.Replace, descriptors);
    }
}
