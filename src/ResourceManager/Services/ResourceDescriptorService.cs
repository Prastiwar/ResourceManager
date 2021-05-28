using ResourceManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager.Services
{
    public class ResourceDescriptorService : IResourceDescriptorService
    {
        protected Dictionary<Type, IResourceDescriptor[]> resources = new Dictionary<Type, IResourceDescriptor[]>();

        public IEnumerable<IResourceDescriptor> Describe(Type type)
        {
            if (resources.TryGetValue(type, out IResourceDescriptor[] typedResources))
            {
                return typedResources;
            }
            throw new MissingDescriptorException($"{type.Name} cannot be described by any descriptor");
        }

        public void Register(Type type, RegisterOptions options, params IResourceDescriptor[] descriptors)
        {
            if (descriptors is null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
            CheckDuplication(descriptors);
            if (resources.TryGetValue(type, out IResourceDescriptor[] currentDescriptors))
            {
                switch (options)
                {
                    case RegisterOptions.Append:
                        resources[type] = currentDescriptors.Concat(descriptors).ToArray();
                        break;
                    case RegisterOptions.Replace:
                        resources[type] = descriptors;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                resources[type] = descriptors;
            }
        }

        private static void CheckDuplication(IResourceDescriptor[] descriptors)
        {
            HashSet<IResourceDescriptor> descriptorsHashset = new HashSet<IResourceDescriptor>(descriptors.Length);
            foreach (IResourceDescriptor item in descriptors)
            {
                bool added = descriptorsHashset.Add(item);
                if (!added)
                {
                    throw new DuplicationException($"Descriptor of type {item.GetType()} is duplicated.");
                }
            }
        }
    }
}
