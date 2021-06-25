using System;
using System.Collections.Generic;
using System.Reflection;

namespace ResourceManager
{
    public class ScannerOptionsBuilder
    {
        protected IList<Assembly> TargetAssemblies { get; } = new List<Assembly>();

        protected HashSet<Type> ExcludedTypes { get; } = new HashSet<Type>();

        public ScannerOptionsBuilder Exclude(params Type[] types)
        {
            foreach (Type type in types)
            {
                ExcludedTypes.Add(type);
            }
            return this;
        }

        public ScannerOptionsBuilder Include(params Type[] types)
        {
            foreach (Type type in types)
            {
                ExcludedTypes.Remove(type);
            }
            return this;
        }

        public ScannerOptionsBuilder Exclude(params Assembly[] assemblies)
        {
            TargetAssemblies.RemoveRange(assemblies);
            return this;
        }

        public ScannerOptionsBuilder Include(params Assembly[] assemblies)
        {
            TargetAssemblies.AddRange(assemblies);
            return this;
        }

        public ScannerOptions Build() => new ScannerOptions(TargetAssemblies, ExcludedTypes);
    }
}
