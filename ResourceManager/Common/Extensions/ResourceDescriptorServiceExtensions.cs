using ResourceManager.Data;
using ResourceManager.Services;
using System;

namespace ResourceManager
{
    public static class ResourceDescriptorServiceExtensions
    {
        public static IResourceDescriptor Create<T>(this IResourceDescriptorService creator, object key = null) => creator.Describe(typeof(T), key);

        public static IResourceDescriptor Create(this IResourceDescriptorService creator, Type type) => creator.Describe(type, null);
    }
}
