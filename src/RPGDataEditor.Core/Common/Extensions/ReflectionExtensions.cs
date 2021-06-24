using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor
{
    public static class ReflectionExtensions
    {
        /// <summary> Enumerates between non-abstract subclasses </summary>
        public static IEnumerable<Type> EnumarateDerivedTypes(this Type baseClass)
        {
            IEnumerable<Type> scanTypes = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).SelectMany(x => x.GetExportedTypes().Where(type => !type.IsAbstract && baseClass.IsAssignableFrom(type)));
            if (baseClass.IsInterface)
            {
                foreach (Type type in scanTypes)
                {
                    yield return type;
                }
            }
            else
            {
                foreach (Type type in scanTypes)
                {
                    if (type.IsSubclassOf(baseClass))
                    {
                        yield return type;
                    }
                }
            }
        }

        public static Type GetArrayElementType(this Type type)
        {
            Type elementType = type.GetElementType();
            if (elementType == null)
            {
                elementType = type.GetGenericArguments().FirstOrDefault();
            }
            return elementType;
        }

    }
}
