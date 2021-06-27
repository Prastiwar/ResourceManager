using Prism.Ioc;
using System;

namespace RPGDataEditor.Extensions.Prism.Wpf.Services
{
    public class PrismServiceProvider : IServiceProvider
    {
        public PrismServiceProvider(IContainerProvider container) => Container = container;

        public IContainerProvider Container { get; }

        public object GetService(Type serviceType) => Container.Resolve(serviceType);
    }
}
