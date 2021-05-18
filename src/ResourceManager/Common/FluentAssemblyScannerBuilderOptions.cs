using System.Reflection;

namespace ResourceManager
{
    public static partial class DependencyInjectionExtensions
    {
        public class FluentAssemblyScannerBuilderOptions
        {
            public bool ScanReferencedAssemblies { get; set; }
            public Assembly[] ScanAssemblies { get; set; }
        }
    }
}
