using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Type baseType = type;
            while (true)
            {
                baseType = baseType.BaseType;
                if (baseType == null)
                {
                    break;
                }
                yield return baseType;
            }
        }

        public static object GetDefaultValue(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

        /// <summary> Returns true for array, IEnumerable, IEnumerable<>, excludes string </summary>
        public static bool IsEnumerable(this Type type) => type.IsArray || (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type));

        public static Type GetEnumerableElementType(this Type type)
        {
            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();
                return arrayElementType;
            }
            Type elementType = type.GetGenericArguments().FirstOrDefault();
            return elementType ?? typeof(object);
        }

        public static Assembly[] GetReferencedAssemblies(this Assembly assembly, bool includeRoot)
        {
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            IList<Assembly> assemblies = new List<Assembly>(includeRoot ? referencedAssemblies.Length : referencedAssemblies.Length + 1);
            if (includeRoot)
            {
                assemblies.Add(assembly);
            }
            for (int i = includeRoot ? 1 : 0; i < referencedAssemblies.Length; i++)
            {
                assemblies.Add(Assembly.Load(referencedAssemblies[i]));
            }
            return assemblies.ToArray();
        }
    }
}
