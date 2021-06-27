using AutoUpdaterDotNET;
using DryIoc;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ResourceManager.Core.Serialization;
using ResourceManager.Core.Services;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Extensions.Prism.Wpf;
using ResourceManager.Extensions.Prism.Wpf.Services;
using ResourceManager.Mvvm.Services;
using ResourceManager.Providers;
using ResourceManager.Wpf.Mvvm;
using ResourceManager.Wpf.Providers;
using ResourceManager.Wpf.Services;
using ResourceManager.Wpf.Views;
using System;
using System.IO;
using System.Windows;

namespace ResourceManager.Wpf
{
    public abstract class ResourceManagerApplication : PrismApplication
    {
        public ResourceManagerApplication()
        {
            DispatcherUnhandledException += OnUnhandledException;
            Exit += OnExit;
        }

        public static new ResourceManagerApplication Current => Application.Current as ResourceManagerApplication;

        protected virtual string SessionFilePath {
            get {
                string path = Path.GetFullPath("./cache");
                Directory.CreateDirectory(path);
                return Path.Combine(path, "session.json");
            }
        }

        protected virtual Func<string> LogFilePathFunc => () => $"./logs/log_{DateTime.Now.ToString("dd_MM_yyyy")}.txt";

        protected virtual void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
            => Container.Resolve<ILogger<ResourceManagerApplication>>().LogError(e.Exception, "Unhandled exception");

        protected virtual void OnExit(object sender, ExitEventArgs e) { }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(System.Windows.Controls.TabControl), Container.Resolve<TabControlAdapter>());
        }

        protected sealed override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            JsonConvert.DefaultSettings = CreateJsonSettings;

            ServiceCollection services = new ServiceCollection();
            Configure(services);
            ServiceProvider provider = services.BuildServiceProvider();
            containerRegistry.RegisterServices(services, provider);
            Configure(containerRegistry);

            if (Container.Resolve<IAutoTemplateProvider>() is DefaultAutoTemplateProvider autoTemplateProvider)
            {
                autoTemplateProvider.RegisterDefaults(containerRegistry);
            }

            InitializeAutoUpdater();
            OnConfigured(provider);
        }

        protected virtual void Configure(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddFile(LogFilePathFunc));

            services.AddConfiguration(builder => builder.AddWriteableJsonFile(SessionFilePath, true, true));

            services.AddFluentAssemblyScanner(null, scanner => {
                services.AddScannedServices(scanner, typeof(IValidator<>), ServiceLifetime.Transient, new ScannerOptionsBuilder().Exclude(typeof(InlineValidator<>)).Build());
            });

            services.AddSingleton<ITextSerializer, NewtonsoftSerializer>();

            services.AddSingleton(typeof(IImplementationProvider<>), typeof(DefaultImplementationProvider<>));
            services.AddSingleton<IServiceProvider, PrismServiceProvider>();

            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IDialogService, PrismDialogService>();
            services.AddSingleton<IViewService, ViewService>();

            services.AddDataSourceConfiguration(builder => ConfigureDataSources(builder), ConfigureDataSource, ClearPreviousDataSource);
        }

        protected abstract void ConfigureDataSources(IConfigurableDataSourceBuilder builder);

        protected virtual void ClearPreviousDataSource(IServiceCollection services, IDataSource previousSource)
        {
            foreach (ServiceDescriptor service in services)
            {
                Container.GetContainer().Unregister(service.ServiceType);
            }
        }

        protected virtual void ConfigureDataSource(IServiceCollection services, IDataSource currentSource)
        {
            IContainerRegistry registry = (IContainerRegistry)Container;
            registry.RegisterServices(services, services.BuildServiceProvider());
        }

        /// <summary> This method is used to save prism compatibility, you should prefer Configure(IServiceCollection services) </summary>
        protected virtual void Configure(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<UpdateDialog>(typeof(UpdateDialog).Name);
            containerRegistry.RegisterDialog<ConnectionDialog>(typeof(ConnectionDialog).Name);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(new DefaultAutoTemplateProvider(Container));
        }

        protected virtual JsonSerializerSettings CreateJsonSettings()
        {
            PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
            propResolver.SetAllLetterCase(Lettercase.CamelCase);
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new ConfigurationSectionJsonConverter());
            return settings;
        }

        protected virtual void InitializeAutoUpdater()
        {
            AutoUpdater.AppCastURL = "https://raw.githubusercontent.com/Prastiwar/ResourceManager/main/version.json";
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdater_ParseUpdateInfoEvent;
        }

        private void AutoUpdater_ParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs() {
                CurrentVersion = updateInfo.Version,
                DownloadURL = updateInfo.Url,
                ChangelogURL = updateInfo.Changelog,
                InstallerArgs = updateInfo.Args,
                Mandatory = updateInfo.Mandatory,
                CheckSum = updateInfo.CheckSum
            };
        }

        protected virtual void OnConfigured(IServiceProvider provider)
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            IConfigurationSection dataSourceSection = configuration.GetDataSourceSection();
            if (!dataSourceSection.GetSection(DataSourceExtensions.NameKey).Exists())
            {
                dataSourceSection[DataSourceExtensions.NameKey] = LocalDataSourceExtensions.Name;
            }

            IConfigurableDataSource configurableDataSource = provider.GetRequiredService<IConfigurableDataSource>();
            configurableDataSource.Configure(dataSourceSection);
            configuration.GetReloadToken().RegisterChangeCallback((config) => configurableDataSource.Configure((IConfiguration)config), dataSourceSection);
        }
    }
}
