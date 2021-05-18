using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Type GetEnumerableElementType(this Type type)
        {
            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();
                return arrayElementType;
            }
            Type elementType = type.GetGenericArguments().FirstOrDefault();
            if (elementType == null)
            {
                return typeof(object);
            }
            return elementType;
        }
    }
}
