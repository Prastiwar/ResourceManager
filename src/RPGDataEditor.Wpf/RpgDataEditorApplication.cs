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
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Extensions.Prism.Wpf.Services;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Providers;
using RPGDataEditor.Services;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Providers;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public abstract class RpgDataEditorApplication : PrismApplication
    {
        public RpgDataEditorApplication()
        {
            DispatcherUnhandledException += OnUnhandledException;
            Exit += OnExit;
        }

        public static new RpgDataEditorApplication Current => Application.Current as RpgDataEditorApplication;

        protected string AppRootPath => new DirectoryInfo(BinaryPath).Parent.FullName;

        protected string BinaryPath => Path.Combine(Environment.CurrentDirectory + "bin");

        protected virtual string CacheDirectoryPath {
            get {
                string path = Path.Combine(AppRootPath, "cache");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        protected virtual void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
            => Container.Resolve<ILogger<RpgDataEditorApplication>>().LogError(e.Exception, "Unhandled exception");

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
            services.AddLogging(builder => builder.AddFile(() => $"./logs/log_{DateTime.Now.ToString("dd_MM_yyyy")}.txt"));

            services.AddConfiguration(builder => builder.AddJsonFile(Path.Combine(CacheDirectoryPath, ConfigurationExtensions.SessionFileName + ".json"), true, true));

            services.AddFluentAssemblyScanner(null, scanner => services.AddScannedServices(scanner, typeof(IValidator<>), ServiceLifetime.Transient));

            services.AddSingleton<ITextSerializer, NewtonsoftSerializer>();
            services.AddSingleton<IAppPersistanceService, LocalAppPersistanceService>(x => new LocalAppPersistanceService(x.GetRequiredService<ITextSerializer>()) { FolderPath = CacheDirectoryPath });

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
            AutoUpdater.AppCastURL = "https://raw.githubusercontent.com/Prastiwar/RPGDataEditor/main/version.json";
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
