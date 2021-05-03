using ResourceManager.Data;
using System;
using System.Collections.Generic;

namespace ResourceManager.Services
{
    public class ResourceDescriptorService : IResourceDescriptorService
    {
        protected Dictionary<object, IResourceDescriptor[]> resources = new Dictionary<object, IResourceDescriptor[]>();

        public IResourceDescriptor Describe(Type type, object key)
        {
            throw new NotImplementedException();
        }
    }
}
