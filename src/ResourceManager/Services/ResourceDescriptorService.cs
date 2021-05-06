﻿using ResourceManager.Data;
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
            throw new DescribtionException($"{type.Name} cannot be described by any descriptor");
        }

        public void Register(Type type, RegisterOptions options, params IResourceDescriptor[] descriptors)
        {
            if (descriptors is null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
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
    }
}