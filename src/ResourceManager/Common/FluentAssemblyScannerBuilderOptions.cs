using System.Reflection;

namespace ResourceManager
{
    public class FluentAssemblyScannerBuilderOptions
    {
        public bool ScanReferencedAssemblies { get; set; }
        public Assembly[] ScanAssemblies { get; set; }
    }
}
