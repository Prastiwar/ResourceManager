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

        public IFluentTypeSelector Scan(Assembly[] assemblies) => CreateSelector(assemblies);

        public IFluentTypeSelector Scan() => Scan(PreservedAssemblies);

        protected virtual IFluentTypeSelector CreateSelector(Assembly[] assemblies) => new FluentTypeSelector(assemblies);
    }
}
