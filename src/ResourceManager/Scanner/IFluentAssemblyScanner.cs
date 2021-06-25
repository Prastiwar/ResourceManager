using System;
using System.Collections.Generic;
using System.Reflection;

namespace ResourceManager
{
    public interface IFluentAssemblyScanner
    {
        HashSet<Type> ExcludedTypes { get; }

        IList<Assembly> ScanAssemblies { get; }

        IFluentTypeSelector Scan(IEnumerable<Assembly> assemblies);
        IFluentTypeSelector Scan();
        IFluentTypeSelector ScanIgnoring(IEnumerable<Assembly> assemblies);
    }
}
