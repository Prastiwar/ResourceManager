using ResourceManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager.Services
{
    public class ResourceDescriptorService : IResourceDescriptorService
    {
        protected Dictionary<Type, IResourceDescriptor[]> registrations = new Dictionary<Type, IResourceDescriptor[]>();

        public IEnumerable<IResourceDescriptor> Describe(Type type)
        {
            if (registrations.TryGetValue(type, out IResourceDescriptor[] typedResources))
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
            if (registrations.TryGetValue(type, out IResourceDescriptor[] currentDescriptors))
            {
                switch (options)
                {
                    case RegisterOptions.Append:
                        registrations[type] = currentDescriptors.Concat(descriptors).ToArray();
                        break;
                    case RegisterOptions.Replace:
                        registrations[type] = descriptors;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                registrations[type] = descriptors;
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

        public IEnumerable<KeyValuePair<Type, IResourceDescriptor[]>> GetRegistrations() => registrations;

    }
}
