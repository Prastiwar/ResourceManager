using System;

namespace RPGDataEditor.Core.Connection
{
    public interface IResourceToTypeConverter
    {
        public Type GetResourceType(int resource);

        public Type GetResourceType(IIdentifiable resource);
    }
}
