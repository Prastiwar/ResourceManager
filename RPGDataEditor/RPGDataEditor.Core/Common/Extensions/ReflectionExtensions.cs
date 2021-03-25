using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Core
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> EnumarateDerivedTypes(this Type baseClass) => from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                                                                      where !domainAssembly.IsDynamic
                                                                                      from assemblyType in domainAssembly.GetExportedTypes()
                                                                                      where baseClass.IsAssignableFrom(assemblyType) &&
                                                                                            !assemblyType.IsAbstract &&
                                                                                            assemblyType.IsSubclassOf(baseClass)
                                                                                      select assemblyType;

    }
}
