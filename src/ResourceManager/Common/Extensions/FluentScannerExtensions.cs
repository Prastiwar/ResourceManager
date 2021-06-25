using System;
using System.Collections.Generic;
using System.Reflection;

namespace ResourceManager
{
    public static class FluentScannerExtensions
    {
        public static IFluentTypeSelector Ignore(this IFluentTypeSelector selector, params Type[] types) => selector.Ignore(new HashSet<Type>(types));

        public static IFluentTypeSelector Select(this IFluentTypeSelector selector, params Type[] types) => selector.Select(types);

        public static IFluentAssemblyScanner Include(this IFluentAssemblyScanner scanner, params Assembly[] assemblies)
        {
            scanner.ScanAssemblies.AddRange(assemblies);
            return scanner;
        }

        public static IFluentAssemblyScanner Ignore(this IFluentAssemblyScanner scanner, params Assembly[] assemblies)
        {
            scanner.ScanAssemblies.RemoveRange(assemblies);
            return scanner;
        }

        public static IFluentAssemblyScanner Include(this IFluentAssemblyScanner scanner, params Type[] types)
        {
            foreach (Type type in types)
            {
                scanner.ExcludedTypes.Remove(type);
            }
            return scanner;
        }

        public static IFluentAssemblyScanner Ignore(this IFluentAssemblyScanner scanner, params Type[] types)
        {
            foreach (Type type in types)
            {
                scanner.ExcludedTypes.Add(type);
            }
            return scanner;
        }

        public static IFluentAssemblyScanner Reset(this IFluentAssemblyScanner scanner)
        {
            ;
            return scanner;
        }

        public static IFluentTypeSelector Scan(this IFluentAssemblyScanner scanner, params Assembly[] assemblies) => scanner.Scan(assemblies);

        public static IFluentTypeSelector ScanIgnoring(this IFluentAssemblyScanner scanner, params Assembly[] assemblies) => scanner.ScanIgnoring(assemblies);
    }
}
