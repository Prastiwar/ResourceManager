using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager.Services
{
    public class AssemblyScannerOptions
    {
        public bool AcceptInterfaces { get; set; }
        public bool AcceptAbstract { get; set; }
        public bool AcceptValueType { get; set; }
        public Type[] CustomAttributes { get; set; }
    }

    public interface IAssemblyScanner
    {
        IEnumerable<Type> ScanDerivedClasses(Type baseType, AssemblyScannerOptions options, params Assembly[] assemblies);

        IEnumerable<Type> ScanGenericClasses(Type ofType, AssemblyScannerOptions options, params Assembly[] assemblies);
    }

    public class TypeScan
    {
        public TypeScan(Type scannedType, IEnumerable<Type> resultTypes)
        {
            ScannedType = scannedType;
            ResultTypes = resultTypes;
        }

        public Type ScannedType { get; }
        public IEnumerable<Type> ResultTypes { get; }
    }

    public class TypeScanResult
    {
        public TypeScanResult(IEnumerable<TypeScan> results) => ResultTypes = results;

        public IEnumerable<TypeScan> ResultTypes { get; }
    }

    public interface IFluentTypeSelector
    {
        IFluentTypeSelector Select(params Type[] types);
        IFluentTypeSelector ScanAbstract(bool isAbstract);
        IFluentTypeSelector ScanInterface(bool isInterface);
        IFluentTypeSelector ScanTypes(Predicate<Type> predicate);
        IFluentTypeSelector ScanTypes(IFluentTypeSelector selector);
        TypeScanResult Get();
    }

    public interface IFluentAssemblyScanner
    {
        IFluentTypeSelector Scan(Assembly[] assemblies);
        IFluentTypeSelector Scan();
    }

    public class FluentTypeSelector : IFluentTypeSelector
    {
        public FluentTypeSelector(Assembly[] assemblies) => Assemblies = assemblies;

        protected IList<Type> TypesToScan { get; private set; } = new List<Type>();

        protected Assembly[] Assemblies { get; }

        private bool selectAbstract;
        private bool selectInterface;

        private Predicate<Type> customPredicate;

        public IFluentTypeSelector ScanTypes(Predicate<Type> predicate)
        {
            customPredicate = predicate;
            return this;
        }

        public IFluentTypeSelector ScanTypes(IFluentTypeSelector selector)
        {
            foreach (TypeScan scan in selector.Get().ResultTypes)
            {
                TypesToScan.AddRange(scan.ResultTypes);
            }
            return this;
        }

        public IFluentTypeSelector Select(params Type[] types)
        {
            TypesToScan.AddRange(types);
            return this;
        }

        public IFluentTypeSelector ScanAbstract(bool isAbstract)
        {
            selectAbstract = isAbstract;
            return this;
        }

        public IFluentTypeSelector ScanInterface(bool isInterface)
        {
            selectInterface = isInterface;
            return this;
        }

        public TypeScanResult Get()
        {
            IEnumerable<KeyValuePair<Type, Type>> singleResults = from scanType in TypesToScan
                                                                  from assembly in Assemblies
                                                                  from assemblyType in assembly.GetExportedTypes()
                                                                  where (HasSimpleRelationship(scanType, assemblyType) || HasGenericRelationship(scanType, assemblyType)) &&
                                                                        (selectAbstract ? assemblyType.IsAbstract : !assemblyType.IsAbstract) &&
                                                                        (customPredicate == null || customPredicate.Invoke(assemblyType))
                                                                  select new KeyValuePair<Type, Type>(scanType, assemblyType);
            return new TypeScanResult(singleResults.GroupBy(x => x.Key, (scanType, results) => new TypeScan(scanType, results.Select(resultTypes => resultTypes.Value))));
        }

        private static bool HasSimpleRelationship(Type scanType, Type assemblyType) => scanType.IsAssignableFrom(assemblyType) && assemblyType.IsSubclassOf(scanType);

        private static bool HasGenericRelationship(Type scanType, Type assemblyType)
        {
            if (!scanType.IsGenericType)
            {
                return false;
            }
            Type baseType = assemblyType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == scanType)
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }
            return false;
        }
    }

    public class FluentAssemblyScanner : IFluentAssemblyScanner
    {
        public FluentAssemblyScanner(Assembly[] assemblies = null)
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
