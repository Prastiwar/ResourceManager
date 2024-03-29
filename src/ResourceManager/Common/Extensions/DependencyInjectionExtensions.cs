﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager
{
    public static class DependencyInjectionExtensions
    {
        private class QueryTypeDuplicationInfo
        {
            protected QueryTypeDuplicationInfo() => Types = new List<Type>();

            public QueryTypeDuplicationInfo(Type queryType) : this() => QueryType = queryType;

            public QueryTypeDuplicationInfo(Type queryType, Type handlerType) : this(queryType) => Types.Add(handlerType);

            public QueryTypeDuplicationInfo(Type queryType, IEnumerable<Type> types)
            {
                QueryType = queryType;
                Types = new List<Type>(types);
            }

            public Type QueryType { get; }

            public IList<Type> Types { get; }

            public int Count => Types.Count;

            public void Add(Type type) => Types.Add(type);
        }

        public static void AssertNoQueryDuplication(this IServiceCollection services, Type serviceType)
        {
            if (!serviceType.IsGenericType || !serviceType.IsGenericTypeDefinition)
            {
                return;
            }
            Dictionary<Type, QueryTypeDuplicationInfo> queryCounter = new Dictionary<Type, QueryTypeDuplicationInfo>();
            foreach (ServiceDescriptor service in services.Where(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition().IsAssignableFrom(serviceType)))
            {
                Type serviceTypeDefinition = null;
                IEnumerable<Type> typesToScan = serviceType.IsInterface ? service.ImplementationType.GetInterfaces() : service.ImplementationType.GetBaseTypes();
                foreach (Type interfaceType in typesToScan)
                {
                    bool isTargetType = interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition().IsAssignableFrom(serviceType) && interfaceType.GetGenericArguments().Length > 0;
                    if (isTargetType)
                    {
                        serviceTypeDefinition = interfaceType;
                        break;
                    }
                }
                if (serviceTypeDefinition == null)
                {
                    continue;
                }
                Type queryType = serviceTypeDefinition.GetGenericArguments()[0];
                if (queryCounter.TryGetValue(queryType, out QueryTypeDuplicationInfo info))
                {
                    info.Add(service.ImplementationType);
                }
                else
                {
                    QueryTypeDuplicationInfo newInfo = new QueryTypeDuplicationInfo(service.ImplementationType);
                    queryCounter[queryType] = newInfo;
                }
            }
            foreach (KeyValuePair<Type, QueryTypeDuplicationInfo> queryEntries in queryCounter)
            {
                if (queryEntries.Value.Count > 1)
                {
                    throw new DuplicationException($"There is duplication of handler for query of type: {queryEntries.Key.Name}, Handlers: {string.Join(", ", queryEntries.Value.Types)}");
                }
            }
        }

        public static void AddScannedServices<TFrom>(this IServiceCollection services, IFluentAssemblyScanner scanner, ServiceLifetime lifetime, ScannerOptions options = null)
            => services.AddScannedServices(scanner, typeof(TFrom), lifetime, options);

        public static void AddScannedServices(this IServiceCollection services, IFluentAssemblyScanner scanner, Type serviceType, ServiceLifetime lifetime, ScannerOptions options = null)
        {
            IFluentTypeSelector selector = null;
            if (options != null)
            {
                selector = options.TargetAssemblies != null && options.TargetAssemblies.Any()
                                            ? scanner.Scan(options.TargetAssemblies)
                                            : scanner.Scan();
                if (options.ExcludedTypes is HashSet<Type> set)
                {
                    selector = selector.Ignore(set);
                }
                else
                {
                    selector = selector.Ignore(new HashSet<Type>(options.ExcludedTypes));
                }
            }
            else
            {
                selector = scanner.Scan();
            }
            TypeScan results = selector.Select(serviceType).Get().Scans.First();
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
                Assembly[] referencedAssemblies = entryAssembly.GetReferencedAssemblies(true);
                assemblies.AddRange(referencedAssemblies);
            }
            if (builderOptions.ScanAssemblies != null)
            {
                assemblies.AddRange(builderOptions.ScanAssemblies);
            }
            FluentAssemblyScanner instance = new FluentAssemblyScanner(new HashSet<Assembly>(assemblies).ToArray());
            scanner?.Invoke(instance);
            services.AddSingleton<IFluentAssemblyScanner>(instance);
        }

        public static void AddDataSourceConfiguration(this IServiceCollection services, Action<IConfigurableDataSourceBuilder> build)
            => AddDataSourceConfiguration(services, new DataSourceWrapperConfigurator(services), build);

        public static void AddDataSourceConfiguration(this IServiceCollection services, Action<IConfigurableDataSourceBuilder> build, Action<IServiceCollection, IDataSource> configure)
            => AddDataSourceConfiguration(services, new DataSourceDelegateConfigurator(configure, null), build);

        public static void AddDataSourceConfiguration(this IServiceCollection services, Action<IConfigurableDataSourceBuilder> build, Action<IServiceCollection, IDataSource> configure, Action<IServiceCollection, IDataSource> unregister)
            => AddDataSourceConfiguration(services, new DataSourceDelegateConfigurator(configure, unregister), build);

        public static void AddDataSourceConfiguration(this IServiceCollection services, IDataSourceServicesConfigurator servicesConfigurator, Action<IConfigurableDataSourceBuilder> configure)
        {
            if (servicesConfigurator is null)
            {
                throw new ArgumentNullException(nameof(servicesConfigurator));
            }

            ConfigurableDataSourceBuilder builder = new ConfigurableDataSourceBuilder(servicesConfigurator, services);
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
