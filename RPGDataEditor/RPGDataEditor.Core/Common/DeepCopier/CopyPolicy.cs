using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepCopy
{
    /// <summary>
    /// Methods for determining the copyability of types and fields.
    /// </summary>
    internal sealed class CopyPolicy
    {
        private enum Policy
        {
            Tracking,
            Mutable,
            Immutable
        }

        private readonly ConcurrentDictionary<Type, Policy> policies = new ConcurrentDictionary<Type, Policy>();
        private readonly RuntimeTypeHandle intPtrTypeHandle = typeof(IntPtr).TypeHandle;
        private readonly RuntimeTypeHandle uIntPtrTypeHandle = typeof(UIntPtr).TypeHandle;
        private readonly Type delegateType = typeof(Delegate);

        public CopyPolicy() => policies[typeof(object)] = Policy.Tracking; // we need to track

        /// <summary>
        /// Returns a sorted list of the copyable fields of the provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A sorted list of the fields of the provided type.</returns>
        public List<FieldInfo> GetCopyableFields(Type type)
        {
            List<FieldInfo> result =
                GetAllFields(type)
                    .Where(field => IsSupportedFieldType(field.FieldType))
                    .ToList();
            result.Sort(FieldInfoComparer.Instance);
            return result;

            IEnumerable<FieldInfo> GetAllFields(Type containingType)
            {
                const BindingFlags allFields =
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
                Type current = containingType;
                while (current != typeof(object) && current != null)
                {
                    FieldInfo[] fields = current.GetFields(allFields);
                    foreach (FieldInfo field in fields)
                    {
                        yield return field;
                    }

                    current = current.BaseType;
                }
            }

            bool IsSupportedFieldType(Type fieldType)
            {
                if (fieldType.IsPointer || fieldType.IsByRef)
                {
                    return false;
                }

                RuntimeTypeHandle handle = fieldType.TypeHandle;
                if (handle.Equals(intPtrTypeHandle))
                {
                    return false;
                }

                if (handle.Equals(uIntPtrTypeHandle))
                {
                    return false;
                }

                if (delegateType.IsAssignableFrom(fieldType))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Returns true if the provided type is immutable, otherwise false.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if the provided type is immutable, otherwise false.</returns>
        public bool IsImmutable(Type type) => GetPolicy(type) == Policy.Immutable;

        public bool NeedsTracking(Type type)
        {
            if (type.IsValueType)
            {
                return false;
            }
            Policy policy = GetPolicy(type);
            // we found something mutable now we need to check if it needs tracking
            if (policy == Policy.Mutable)
            {
                List<FieldInfo> copyableFields = GetCopyableFields(type);
                Queue<FieldInfo> queue = new Queue<FieldInfo>(copyableFields);
                HashSet<Type> duplicateCheck = new HashSet<Type>(AssignableFromEqualityComparer.Instance) { type }; // add root
                while (queue.Count > 0)
                {
                    FieldInfo current = queue.Dequeue();
                    Type fieldType = current.FieldType;
                    Policy fieldPolicy = GetPolicy(fieldType);
                    if (fieldPolicy == Policy.Immutable)
                    {
                        continue;
                    }
                    if (fieldPolicy == Policy.Tracking)
                    {
                        policies[type] = Policy.Tracking;
                        return true;
                    }
                    if (duplicateCheck.Add(fieldType))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(fieldType)) // Rule 5: enumerable mutable fields need tracking
                        {
                            policies[type] = Policy.Tracking;
                            return true;
                        }
                        List<FieldInfo> fieldFields = GetCopyableFields(fieldType);
                        foreach (FieldInfo fieldField in fieldFields)
                        {
                            queue.Enqueue(fieldField); // Rule 6: Recursive
                        }
                    }
                    else
                    {
                        policies[type] = Policy.Tracking; // Rule 4: one or more mutable fields of the same type
                        return true;
                    }
                }

            }
            return GetPolicy(type) == Policy.Tracking;
        }

        private Policy GetPolicy(Type type)
        {
            if (policies.TryGetValue(type, out Policy result))
            {
                return result;
            }

            if (type.GetCustomAttribute<ImmutableAttribute>(false) != null)
            {
                return policies[type] = Policy.Immutable;
            }

            // Rule 1: primitives and quasi primitves
            if (type.IsPrimitive || type.IsEnum || type.IsPointer || type == typeof(string))
            {
                return policies[type] = Policy.Immutable;
            }

            // covers interface and abstract type too
            if (!type.IsSealed)
            {
                return policies[type] = Policy.Mutable;
            }

            if (type.IsArray)
            {
                return policies[type] = Policy.Mutable;
            }
            // Rule 1,2
            if (type.IsValueType)
            {
                List<FieldInfo> copyableFields = GetCopyableFields(type);
                foreach (FieldInfo copyableField in copyableFields)
                {
                    Type fieldType = copyableField.FieldType;
                    if (GetPolicy(fieldType) != Policy.Immutable)
                    {
                        return policies[type] = Policy.Mutable;
                    }
                }
            }
            // Rule 3
            else if (type.IsClass)
            {
                List<FieldInfo> copyableFields = GetCopyableFields(type);
                foreach (FieldInfo copyableField in copyableFields)
                {
                    if (!copyableField.IsInitOnly)
                    {
                        return policies[type] = Policy.Mutable;
                    }

                    Type fieldType = copyableField.FieldType;
                    if (GetPolicy(fieldType) != Policy.Immutable)
                    {
                        return policies[type] = Policy.Mutable;
                    }
                }
            }

            return policies[type] = Policy.Immutable;
        }

        /// <summary>
        /// A comparer for <see cref="FieldInfo"/> which compares by name.
        /// </summary>
        private sealed class FieldInfoComparer : IComparer<FieldInfo>
        {
            /// <summary>
            /// Gets the singleton instance of this class.
            /// </summary>
            public static FieldInfoComparer Instance { get; } = new FieldInfoComparer();

            /// <inheritdoc />
            public int Compare(FieldInfo x, FieldInfo y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        private sealed class AssignableFromEqualityComparer : IEqualityComparer<Type>
        {
            public static AssignableFromEqualityComparer Instance { get; } = new AssignableFromEqualityComparer();
            private static readonly Type ObjectType = typeof(object);
            public bool Equals(Type x, Type y)
            {
                // We can't reason about object
                if (x == ObjectType || y == ObjectType)
                {
                    return false;
                }
                return x.IsAssignableFrom(y) || y.IsAssignableFrom(x);
            }

            public int GetHashCode(Type obj) => 0;
        }

    }
}
