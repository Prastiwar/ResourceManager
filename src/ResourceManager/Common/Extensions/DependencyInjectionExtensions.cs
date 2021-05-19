using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.DataSource;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public static partial class DependencyInjectionExtensions
    {
        public static void AddFluentMediatr(this IServiceCollection services, IFluentAssemblyScanner scanner)
        {
            services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
            services.AddScannedServices(scanner, typeof(IRequestHandler<>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestHandler<,>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(INotificationHandler<>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestPreProcessor<>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestPostProcessor<,>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestExceptionHandler<,>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestExceptionHandler<,,>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestExceptionAction<>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IRequestExceptionAction<,>), ServiceLifetime.Transient);
            services.AddScannedServices(scanner, typeof(IPipelineBehavior<,>), ServiceLifetime.Transient);
        }

        public static void AddScannedServices<TFrom>(this IServiceCollection services, IFluentAssemblyScanner scanner, ServiceLifetime lifetime)
            => services.AddScannedServices(scanner, typeof(TFrom), lifetime);

        public static void AddScannedServices(this IServiceCollection services, IFluentAssemblyScanner scanner, Type serviceType, ServiceLifetime lifetime)
        {
            TypeScan results = scanner.Scan().Select(serviceType).Get().Scans.First();
            foreach (Type implementationType in results.ResultTypes)
            {
                if (serviceType.IsGenericTypeDefinition)
                {
                    IEnumerable<Type> conreteServiceTypes = (serviceType.IsInterface ? implementationType.GetInterfaces() : implementationType.GetBaseTypes())
                                                                      .Where(i => i.IsGenericType && serviceType.IsAssignableFrom(i.GetGenericTypeDefinition()));
                    foreach (Type concreteServiceType in conreteServiceTypes)
                    {
                        Type registerType = concreteServiceType;
                        if (concreteServiceType.FullName == null)
                        {
                            registerType = concreteServiceType.GetGenericTypeDefinition();
                            if (implementationType.IsGenericTypeDefinition)
                            {
                                if (implementationType.GetGenericArguments().Length != registerType.GetGenericArguments().Length)
                                {
                                    continue;
                                }
                            }
                        }
                        services.Add(new ServiceDescriptor(registerType, implementationType, lifetime));
                    }
                }
                else
                {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
                }
            }
        }

        public static void AddResourceDescriptor(this IServiceCollection services, Action<IResourceDescriptorService> builder)
        {
            ResourceDescriptorService service = new ResourceDescriptorService();
            builder?.Invoke(service);
            services.AddSingleton<IResourceDescriptorService>(service);
        }

        public static void AddFluentAssemblyScanner(this IServiceCollection services, Action<FluentAssemblyScannerBuilderOptions> options = null, Action<IFluentAssemblyScanner> scanner = null)
        {
            FluentAssemblyScannerBuilderOptions builderOptions = new FluentAssemblyScannerBuilderOptions() {
                ScanReferencedAssemblies = true
            };
            options?.Invoke(builderOptions);
            List<Assembly> assemblies = new List<Assembly>();
            if (builderOptions.ScanReferencedAssemblies)
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                AssemblyName[] referencedAssemblies = entryAssembly.GetReferencedAssemblies();
                assemblies.Add(entryAssembly);
                for (int i = 1; i < referencedAssemblies.Length; i++)
                {
                    assemblies.Add(Assembly.Load(referencedAssemblies[i]));
                }
            }
            if (builderOptions.ScanAssemblies != null)
            {
                foreach (Assembly assembly in builderOptions.ScanAssemblies)
                {
                    assemblies.Add(assembly);
                }
            }
            FluentAssemblyScanner instance = new FluentAssemblyScanner(assemblies.ToArray());
            scanner?.Invoke(instance);
            services.AddSingleton<IFluentAssemblyScanner>(instance);
        }

        public static void AddDataSourceConfiguration(this IServiceCollection services, Action<IConfigurableDataSourceBuilder> configure)
        {
            ConfigurableDataSourceBuilder builder = new ConfigurableDataSourceBuilder();
            configure.Invoke(builder);
            IConfigurableDataSource configurator = builder.Build();

            services.AddSingleton(configurator);
            services.AddSingleton<IDataSource>(configurator);
        }

        public static void AddConfiguration(this IServiceCollection services, Action<IConfigurationBuilder> configure)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            configure.Invoke(builder);
            IConfigurationRoot configurationRoot = builder.Build();

            services.AddSingleton(configurationRoot);
            services.AddSingleton<IConfiguration>(configurationRoot);
        }
    }
}
