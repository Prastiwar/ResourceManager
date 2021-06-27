using System;

namespace ResourceManager.Data
{
    public interface IResourceDescriptor
    {
        bool CanDescribe<T>(T resource);

        bool CanDescribe(Type resourceType);
    }
}
