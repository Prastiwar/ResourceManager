﻿using ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager.Providers
{
    public class DefaultImplementationProvider<T> : IImplementationProvider<T> where T : class
    {
        public DefaultImplementationProvider(IFluentAssemblyScanner scanner) => Scanner = scanner;

        protected virtual HashSet<Type> GetIgnoredTypes() => new HashSet<Type>();

        protected IFluentAssemblyScanner Scanner { get; }

        public T Get()
        {
            try
            {
                Type implementationType = GetImplementationType(typeof(T));
                return Get(implementationType);
            }
            catch (Exception ex)
            {
                throw new TypeResolveException("Couldn't resolve implementation type of " + typeof(T), ex);
            }
        }

        public virtual T Get(Type targetType)
        {
            if (targetType is null)
            {
                return null;
            }
            if (targetType.IsAbstract)
            {
                return Get(GetImplementationType(targetType));
            }
            return (T)Activator.CreateInstance(targetType);
        }

        protected virtual Type GetImplementationType(Type baseType) => Scanner.Scan().Select(baseType).Ignore(GetIgnoredTypes()).Get().Scans.First().ResultTypes.First();
    }
}
