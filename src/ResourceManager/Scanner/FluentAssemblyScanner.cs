using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public class FluentAssemblyScanner : IFluentAssemblyScanner
    {
        public FluentAssemblyScanner(params Assembly[] assemblies)
        {
            PreservedAssemblies = assemblies;
            if (PreservedAssemblies == null || PreservedAssemblies.Length == 0)
            {
                PreservedAssemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }
        }

        protected Assembly[] PreservedAssemblies { get; }

        protected IList<Assembly> IncludeAssemblies { get; } = new List<Assembly>();

        protected IList<Assembly> ExcludeAssemblies { get; } = new List<Assembly>();

        public IFluentTypeSelector Scan(Assembly[] assemblies) => CreateSelector(assemblies);

        public IFluentTypeSelector Scan()
        {
            Assembly[] scanAssemblies = PreservedAssemblies.Union(IncludeAssemblies)
                                                           .Except(ExcludeAssemblies)
                                                           .Distinct()
                                                           .ToArray();
            return Scan(scanAssemblies);
        }

        public IFluentTypeSelector Ignore(Assembly[] assemblies)
        {
            Assembly[] scanAssemblies = PreservedAssemblies.Union(IncludeAssemblies)
                                                           .Except(assemblies)
                                                           .Except(ExcludeAssemblies)
                                                           .Distinct()
                                                           .ToArray();
            return Scan(scanAssemblies);
        }

        public IFluentAssemblyScanner Include(Assembly[] assemblies)
        {
            IncludeAssemblies.AddRange(assemblies);
            return this;
        }

        public IFluentAssemblyScanner IncludeIgnore(Assembly[] assemblies)
        {
            ExcludeAssemblies.AddRange(assemblies);
            return this;
        }

        public IFluentAssemblyScanner Reset()
        {
            IncludeAssemblies.Clear();
            ExcludeAssemblies.Clear();
            return this;
        }

        protected virtual IFluentTypeSelector CreateSelector(Assembly[] assemblies) => new FluentTypeSelector(assemblies);
    }
}
