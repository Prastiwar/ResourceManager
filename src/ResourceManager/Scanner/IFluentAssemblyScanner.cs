using System.Reflection;

namespace ResourceManager
{
    public interface IFluentAssemblyScanner
    {
        IFluentAssemblyScanner Include(Assembly[] assemblies);
        IFluentAssemblyScanner IncludeIgnore(Assembly[] assemblies);
        IFluentAssemblyScanner Reset();

        IFluentTypeSelector Scan(Assembly[] assemblies);
        IFluentTypeSelector Scan();
        IFluentTypeSelector Ignore(Assembly[] assemblies);
    }
}
