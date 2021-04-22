using ResourceManager.Data;
using System;

namespace ResourceManager.Services
{
    public interface IResourceDescriptorService
    {
        IResourceDescriptor Describe(Type type, object key);
    }
}
