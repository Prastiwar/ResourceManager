using System.Reflection;

namespace ResourceManager
{
    public interface IFluentAssemblyScanner
    {
        IFluentTypeSelector Scan(Assembly[] assemblies);
        IFluentTypeSelector Scan();
    }
}
