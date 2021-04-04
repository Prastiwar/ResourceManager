using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Core
{
    public static class ReflectionExtensions
    {
        /// <summary> Enumerates between non-abstract subclasses </summary>
        public static IEnumerable<Type> EnumarateDerivedTypes(this Type baseClass) => from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                                                                      where !domainAssembly.IsDynamic
                                                                                      from assemblyType in domainAssembly.GetExportedTypes()
                                                                                      where baseClass.IsAssignableFrom(assemblyType) &&
                                                                                            !assemblyType.IsAbstract &&
                                                                                            assemblyType.IsSubclassOf(baseClass)
                                                                                      select assemblyType;

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
