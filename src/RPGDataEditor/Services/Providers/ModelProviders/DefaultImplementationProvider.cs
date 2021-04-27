using ResourceManager;
using System;
using System.Linq;

namespace RPGDataEditor.Providers
{
    public class DefaultImplementationProvider<T> : IImplementationProvider<T> where T : class
    {
        public DefaultImplementationProvider(IFluentAssemblyScanner scanner) => Scanner = scanner;

        protected IFluentAssemblyScanner Scanner { get; }

        public T Get()
        {
            try
            {
                Type implementationType = GetImplementationType();
                return (T)Activator.CreateInstance(implementationType);
            }
            catch (Exception ex)
            {
                throw new TypeResolveException("Couldn't resolve implementation type of " + typeof(T), ex);
            }
        }

        protected virtual Type GetImplementationType() => Scanner.Scan().Select(typeof(T)).Get().ResultTypes.First().ResultTypes.First();
    }
}
