using System;
using System.Linq;

namespace ResourceManager
{
    public static class ReflectionExtensions
    {
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
