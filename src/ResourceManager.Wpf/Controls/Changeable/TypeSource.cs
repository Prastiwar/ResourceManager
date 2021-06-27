using System;
using System.Collections.Generic;

namespace ResourceManager.Wpf.Controls
{
    public class TypeSource : IEquatable<TypeSource>
    {
        public TypeSource(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public Type Type { get; set; }

        public override bool Equals(object obj) => Equals(obj as TypeSource);

        public bool Equals(TypeSource other) => other != null && Name == other.Name && EqualityComparer<Type>.Default.Equals(Type, other.Type);

        public override int GetHashCode() => HashCode.Combine(Name, Type);

        public static bool operator ==(TypeSource left, TypeSource right) => EqualityComparer<TypeSource>.Default.Equals(left, right);

        public static bool operator !=(TypeSource left, TypeSource right) => !(left == right);
    }
}
