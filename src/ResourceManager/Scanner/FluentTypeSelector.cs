using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public class FluentTypeSelector : IFluentTypeSelector
    {
        public FluentTypeSelector(Assembly[] assemblies) => Assemblies = assemblies;

        protected IList<Type> TypesToScan { get; private set; } = new List<Type>();

        protected Assembly[] Assemblies { get; }

        protected HashSet<Type> IgnoredTypes { get; set; }

        private bool selectAbstract;
        private bool selectInterface;

        private Predicate<Type> customPredicate;

        public IFluentTypeSelector Ignore(HashSet<Type> types)
        {
            IgnoredTypes = types ?? new HashSet<Type>();
            return this;
        }

        public IFluentTypeSelector Ignore(params Type[] types)
        {
            IgnoredTypes = new HashSet<Type>(types);
            return this;
        }

        public IFluentTypeSelector ScanTypes(Predicate<Type> predicate)
        {
            customPredicate = predicate;
            return this;
        }

        public IFluentTypeSelector ScanTypes(IFluentTypeSelector selector)
        {
            foreach (TypeScan scan in selector.Get().ResultTypes)
            {
                foreach (Type item in scan.ResultTypes)
                {
                    TypesToScan.Add(item);
                }
            }
            return this;
        }

        public IFluentTypeSelector Select(params Type[] types)
        {
            foreach (Type item in types)
            {
                TypesToScan.Add(item);
            }
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
            IEnumerable<KeyValuePair<Type, Type>> singleResults = QueryAssemblies();
            IEnumerable<KeyValuePair<Type, Type>> QueryAssemblies()
            {
                foreach (Type scanType in TypesToScan)
                {
                    foreach (Assembly assembly in Assemblies)
                    {
                        foreach (Type assemblyType in assembly.GetExportedTypes())
                        {
                            bool acceptByAbstract = selectAbstract ? assemblyType.IsAbstract : !assemblyType.IsAbstract;
                            if (acceptByAbstract && (customPredicate == null || customPredicate.Invoke(assemblyType)))
                            {
                                if (HasSimpleRelationship(scanType, assemblyType))
                                {
                                    yield return new KeyValuePair<Type, Type>(scanType, assemblyType);
                                }
                                else if (HasGenericRelationship(scanType, assemblyType, out Type genericAssemblyType))
                                {
                                    yield return new KeyValuePair<Type, Type>(scanType, genericAssemblyType ?? assemblyType);
                                }
                            }
                        }
                    }
                }
            }
            return new TypeScanResult(singleResults.GroupBy(x => x.Key, (scanType, results) => new TypeScan(scanType, results.Select(resultTypes => resultTypes.Value))));
        }

        private static bool HasGenericRelationship(Type scanType, Type assemblyType, out Type genericType)
        {
            genericType = null;
            if (!scanType.IsGenericType)
            {
                return false;
            }
            Type baseType = assemblyType;
            while (baseType != null)
            {
                if (GetGenericType(scanType, assemblyType, baseType, out genericType))
                {
                    return true;
                }

                foreach (Type interfaceType in baseType.GetInterfaces())
                {
                    if (GetGenericType(scanType, assemblyType, interfaceType, out genericType))
                    {
                        return true;
                    }
                }
                baseType = baseType.BaseType;
            }
            return false;
        }

        private static bool GetGenericType(Type scanType, Type assemblyType, Type checkType, out Type genericType)
        {
            genericType = null;
            if (checkType.IsGenericType)
            {
                bool isEqualToScan = checkType == scanType;
                bool isGenericEqualToScan = checkType.GetGenericTypeDefinition() == scanType;
                bool isEqualToGenericScan = checkType.GetGenericTypeDefinition() == scanType.GetGenericTypeDefinition();
                if (isEqualToGenericScan)
                {
                    if (assemblyType.GetGenericArguments().Length == scanType.GetGenericArguments().Length)
                    {
                        try
                        {
                            genericType = assemblyType.MakeGenericType(scanType.GetGenericArguments());
                        }
                        catch (ArgumentException)
                        {
                            // violates contrains, so it can be made into this generic
                            return false;
                        }
                    }
                }
                if (isEqualToScan || isGenericEqualToScan || isEqualToGenericScan)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasSimpleRelationship(Type scanType, Type assemblyType) => scanType.IsAssignableFrom(assemblyType) && (scanType.IsInterface || assemblyType.IsSubclassOf(scanType));
    }
}
