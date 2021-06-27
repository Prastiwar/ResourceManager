using System;
using System.Collections.Generic;
using System.Reflection;

namespace ResourceManager
{
    public class ScannerOptions
    {
        public ScannerOptions(IEnumerable<Assembly> targetAssemblies, IEnumerable<Type> excludedTypes)
        {
            TargetAssemblies = targetAssemblies;
            ExcludedTypes = excludedTypes;
        }

        public IEnumerable<Assembly> TargetAssemblies { get; }
        public IEnumerable<Type> ExcludedTypes { get; }
    }
}
