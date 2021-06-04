using System;
using System.Collections.Generic;

namespace ResourceManager.Data
{
    /// <summary>
    /// Property contains information about Name, Type of property and Value
    /// DeclaredType is equal to Type unless constraints for creating property were not met 
    /// so there was conversion made from DeclaredType to IResource type
    /// </summary>
    public class ResourceProperty : IEquatable<ResourceProperty>
    {
        public ResourceProperty(string name, Type type, object value = default) : this(name, type, type, value) { }

        public ResourceProperty(string name, Type type, Type declaredType, object value)
        {
            Name = name;
            Type = type;
            DeclaredType = declaredType;
            Value = value;
        }

        public string Name { get; }

        public Type Type { get; }

        public Type DeclaredType { get; }

        public object Value { get; set; }

        public override int GetHashCode() => HashCode.Combine(Name, Type, Value);

        public override bool Equals(object obj) => Equals(obj as ResourceProperty);

        public bool Equals(ResourceProperty other) => other != null &&
                                                         EqualityComparer<string>.Default.Equals(Name, other.Name) &&
                                                         EqualityComparer<Type>.Default.Equals(Type, other.Type) &&
                                                         EqualityComparer<Type>.Default.Equals(DeclaredType, other.DeclaredType) &&
                                                         EqualityComparer<object>.Default.Equals(Value, other.Value);

        public static bool operator ==(ResourceProperty left, ResourceProperty right) => EqualityComparer<ResourceProperty>.Default.Equals(left, right);
        public static bool operator !=(ResourceProperty left, ResourceProperty right) => !(left == right);
    }
}
