using Microsoft.Extensions.DependencyInjection;
using Prism;
using Prism.Ioc;
using System;
using System.Windows;

namespace ResourceManager.Extensions.Prism.Wpf
{
    public static class ApplicationExtensions
    {
        public static object TryResolve(this Application app, Type type)
        {
            try
            {
                return app is PrismApplicationBase prismApp ? prismApp.Container.Resolve(type) : default;
            }
            catch (ContainerResolutionException ex)
            {
                return null;
            }
        }

        public static T TryResolve<T>(this Application app) => TryResolve(app, typeof(T)) is T item ? item : default;

        public static void RegisterServices(this IContainerRegistry containerRegistry, IServiceCollection services, IServiceProvider provider)
        {
            foreach (ServiceDescriptor service in services)
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (service.ImplementationInstance != null)
                        {
                            containerRegistry.RegisterInstance(service.ServiceType, service.ImplementationInstance);
                        }
                        else
                        {
                            if (service.ImplementationFactory != null)
                            {
                                containerRegistry.RegisterSingleton(service.ServiceType, c => service.ImplementationFactory.Invoke(provider));
                            }
                            else
                            {
                                containerRegistry.RegisterSingleton(service.ServiceType, service.ImplementationType);
                            }
                        }
                        break;
                    case ServiceLifetime.Scoped:
                        if (service.ImplementationFactory != null)
                        {
                            containerRegistry.RegisterScoped(service.ServiceType, c => service.ImplementationFactory.Invoke(provider));
                        }
                        else
                        {
                            containerRegistry.RegisterScoped(service.ServiceType, service.ImplementationType);
                        }
                        break;
                    case ServiceLifetime.Transient:
                        if (service.ImplementationFactory != null)
                        {
                            containerRegistry.Register(service.ServiceType, c => service.ImplementationFactory.Invoke(provider));
                        }
                        else
                        {
                            containerRegistry.Register(service.ServiceType, service.ImplementationType);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
