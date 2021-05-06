using ResourceManager.Data;
using System;
using System.Collections.Generic;

namespace ResourceManager.Services
{
    public interface IResourceDescriptorService
    {
        IEnumerable<IResourceDescriptor> Describe(Type type);
    }
}
