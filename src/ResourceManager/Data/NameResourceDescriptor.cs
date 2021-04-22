using System;

namespace ResourceManager.Data
{
    public class NameResourceDescriptor : ResourceDescriptor
    {
        public NameResourceDescriptor(Type type, string name) : base(type) => Name = name;

        public string Name { get; protected set; }

        /// <summary> Creates relative full path by filling RelativeFullPathFormat arguments with <paramref name="resource"/> properties </summary>
        /// <param name="resource"> Target object with arguments as properties </param>
        /// <returns> Relative Full Path to <paramref name="resource"/> using <paramref name="RelativeFullPathFormat"/> </returns>
        public virtual string GetResourceName(object resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }
            bool canBeNamed = Type.IsAssignableFrom(resource.GetType());
            if (canBeNamed)
            {
                return Name;
            }
            return null;
        }
    }
}
