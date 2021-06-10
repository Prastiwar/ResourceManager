using ResourceManager.Data;
using System;
using System.Collections.Generic;

namespace ResourceManager.Services
{
    public interface IResourceDescriptorService
    {
        IEnumerable<IResourceDescriptor> Describe(Type type);

        void Register(Type type, RegisterOptions options, params IResourceDescriptor[] descriptors);

        IEnumerable<KeyValuePair<Type, IResourceDescriptor[]>> GetRegistrations();
    }
}
