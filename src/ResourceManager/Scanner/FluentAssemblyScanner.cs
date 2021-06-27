using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public class FluentAssemblyScanner : IFluentAssemblyScanner
    {
        public FluentAssemblyScanner(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                ScanAssemblies = new List<Assembly>() {
                    Assembly.GetEntryAssembly()
                };
            }
            else
            {
                ScanAssemblies = new List<Assembly>(assemblies);
            }
        }

        public HashSet<Type> ExcludedTypes { get; } = new HashSet<Type>();

        public IList<Assembly> ScanAssemblies { get; } = new List<Assembly>();

        public IFluentTypeSelector Scan(IEnumerable<Assembly> assemblies) => CreateSelector(assemblies);

        public IFluentTypeSelector Scan() => Scan(ScanAssemblies);

        public IFluentTypeSelector ScanIgnoring(IEnumerable<Assembly> assemblies) => Scan(ScanAssemblies.Except(assemblies));

        protected virtual IFluentTypeSelector CreateSelector(IEnumerable<Assembly> assemblies) => new FluentTypeSelector(assemblies).Ignore(ExcludedTypes);
    }
}
