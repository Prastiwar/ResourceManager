using System;
using System.Collections.Generic;
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
        private ModifyGenericHandler genericHandler;

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

        public IFluentTypeSelector ModifyGeneric(ModifyGenericHandler handler)
        {
            genericHandler = handler;
            return this;
        }

        public IFluentTypeSelector ScanTypes(Predicate<Type> predicate)
        {
            customPredicate = predicate;
            return this;
        }

        public IFluentTypeSelector ScanTypes(IFluentTypeSelector selector)
        {
            foreach (TypeScan scan in selector.Get().Scans)
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

        public IFluentTypeSelector Reset()
        {
            selectAbstract = false;
            selectInterface = false;
            genericHandler = null;
            customPredicate = null;
            TypesToScan.Clear();
            IgnoredTypes.Clear();
            return this;
        }

        public TypeScanResult Get()
        {
            IEnumerable<TypeScan> QueryAssemblies()
            {
                foreach (Type scanType in TypesToScan)
                {
                    TypeScan scan = new TypeScan(scanType, EnumerateResultsForType(scanType));
                    yield return scan;
                }
            }
            return new TypeScanResult(QueryAssemblies());
        }

        protected virtual IEnumerable<Type> EnumerateResultsForType(Type scanType)
        {
            foreach (Assembly assembly in Assemblies)
            {
                foreach (Type assemblyType in assembly.GetExportedTypes())
                {
                    if (IgnoredTypes != null && IgnoredTypes.Contains(assemblyType))
                    {
                        continue;
                    }
                    if (assemblyType.IsInterface && !selectInterface)
                    {
                        continue;
                    }
                    bool acceptByAbstract = selectAbstract ? assemblyType.IsAbstract : !assemblyType.IsAbstract;
                    if (acceptByAbstract && (customPredicate == null || customPredicate.Invoke(assemblyType)))
                    {
                        if (HasSimpleRelationship(scanType, assemblyType))
                        {
                            yield return assemblyType;
                        }
                        else if (HasGenericRelationship(scanType, assemblyType, out Type genericAssemblyType))
                        {
                            if (genericHandler != null && genericAssemblyType != null)
                            {
                                yield return genericHandler(scanType, assemblyType, genericAssemblyType);
                            }
                            else
                            {
                                yield return genericAssemblyType ?? assemblyType;
                            }
                        }
                    }
                }
            }
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
                    try
                    {
                        if (scanType.IsGenericTypeDefinition && assemblyType.IsGenericType)
                        {
                            genericType = assemblyType.IsGenericTypeDefinition ? assemblyType : assemblyType.GetGenericTypeDefinition();
                        }
                        else if (assemblyType.GetGenericArguments().Length == scanType.GetGenericArguments().Length)
                        {
                            genericType = assemblyType.MakeGenericType(scanType.GetGenericArguments());
                        }
                    }
                    catch (ArgumentException)
                    {
                        // violates contrains, so it cant be made into this generic
                        return false;
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
