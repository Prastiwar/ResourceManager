﻿using AutoUpdaterDotNET;
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
using ResourceManager.Services;
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
    public class RpgDataEditorApplication : PrismApplication
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
            => Container.Resolve<ILogger>().LogError(e.Exception, "Unhandled exception");

        protected virtual void OnExit(object sender, ExitEventArgs e) { }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(System.Windows.Controls.TabControl), Container.Resolve<TabControlAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();

        protected sealed override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            JsonConvert.DefaultSettings = CreateJsonSettings;

            ServiceCollection services = new ServiceCollection();
            Configure(services);
            ServiceProvider provider = services.BuildServiceProvider();
            containerRegistry.RegisterServices(services, provider);
            Configure(containerRegistry);

            InitializeAutoUpdater();
            OnConfigured(containerRegistry);
        }

        protected virtual void Configure(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddFile(() => $"./logs_{DateTime.Now.ToString("dd_MM_yyyy")}.txt"));

            services.AddConfiguration(builder => builder.AddJsonFile(Path.Combine(CacheDirectoryPath, ConfigurationExtensions.SessionFileName + ".json"), true, true));

            services.AddDataSourceConfiguration(builder => builder.AddLocalDataSource().AddFtpDataSource().AddSqlDataSource(), ConfigureDataSource);

            services.AddFluentAssemblyScanner(null, scanner => {
                services.AddFluentMediatr(scanner);
                services.AddScannedServices(scanner, typeof(IValidator<>), ServiceLifetime.Transient);
            });

            services.AddResourceDescriptor(service => {
                IResourceDescriptor fileQuestDescriptor = new FileQuestPathResourceDescriptor();
                IResourceDescriptor sqlQuestDescriptor = new SqlQuestPathResourceDescriptor();
                service.Register<Models.Quest>(fileQuestDescriptor, sqlQuestDescriptor);

                IResourceDescriptor fileDialogueDescriptor = new FileDialoguePathResourceDescriptor();
                IResourceDescriptor sqlDialogueDescriptor = new SqlDialoguePathResourceDescriptor();
                service.Register<Models.Dialogue>(fileDialogueDescriptor, sqlDialogueDescriptor);

                IResourceDescriptor fileNpcDescriptor = new FileNpcPathResourceDescriptor();
                IResourceDescriptor sqlNpcDescriptor = new SqlNpcPathResourceDescriptor();
                service.Register<Models.Npc>(fileNpcDescriptor, sqlNpcDescriptor);
            });

            services.AddSingleton<ITextSerializer, NewtonsoftSerializer>();
            services.AddSingleton<IAppPersistanceService, LocalAppPersistanceService>();

            services.AddSingleton(typeof(IImplementationProvider<>), typeof(DefaultImplementationProvider<>));
            services.AddSingleton<IServiceProvider, PrismServiceProvider>();

            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IDialogService, PrismDialogService>();
        }

        protected virtual void ConfigureDataSource(IServiceCollection services, IDataSource currentSource)
        {
            // TODO: Add services from Container

            services.AddFluentAssemblyScanner(options => {
                options.ScanReferencedAssemblies = false;
                options.ScanAssemblies = new System.Reflection.Assembly[] {
                    currentSource.GetType().Assembly,
                    typeof(MediatR.Mediator).Assembly,
                    typeof(ResourceManager.Services.MutableServiceFactory).Assembly,
                    typeof(RPGDataEditor.Wpf.RpgDataEditorApplication).Assembly,
                    typeof(RPGDataEditor.Mvvm.BindableClass).Assembly,
                    typeof(RPGDataEditor.Extensions.Prism.Wpf.DialogExtensions).Assembly,
                    typeof(RPGDataEditor.Models.AttributeData).Assembly,
                    typeof(RPGDataEditor.Core.Commands.ValidateResourceHandler).Assembly
                };
            },
            scanner => services.AddFluentMediatr(scanner));
            MutableServiceFactory factory = Container.Resolve<MutableServiceFactory>();
            factory.Factory = services.BuildServiceProvider().GetService;
        }

        protected virtual void Configure(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<UpdateDialog>(typeof(UpdateDialog).Name);
            containerRegistry.RegisterDialog<ConnectionDialog>(typeof(ConnectionDialog).Name);

            AutoTemplateProvider controlProvider = new AutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
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
            settings.Converters.Add(new PlayerRequirementJsonConverter());

            settings.Converters.Add(new NpcJobJsonConverter());
            settings.Converters.Add(new NpcJsonConverter());
            settings.Converters.Add(new TradeItemJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new QuestTaskJsonConverter());
            settings.Converters.Add(new QuestJsonConverter());

            settings.Converters.Add(new DialogueJsonConverter());
            settings.Converters.Add(new DialogueOptionJsonConverter());
            settings.Converters.Add(new TalkDataJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());

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

        protected virtual void OnConfigured(IContainerRegistry containerRegistry)
        {
            IConfiguration configuration = containerRegistry.GetContainer().Resolve<IConfiguration>();
            if (containerRegistry.GetContainer().Resolve<IAppPersistanceService>() is LocalAppPersistanceService service)
            {
                service.FolderPath = CacheDirectoryPath;
            }
            IConfigurationSection dataSourceSection = configuration.GetDataSourceSection();
            if (!dataSourceSection.GetSection(DataSourceExtensions.NameKey).Exists())
            {
                dataSourceSection[DataSourceExtensions.NameKey] = LocalDataSourceExtensions.Name;
            }

            IConfigurableDataSource configurableDataSource = Container.Resolve<IConfigurableDataSource>();
            configurableDataSource.Configure(dataSourceSection);
            configuration.GetReloadToken().RegisterChangeCallback((config) => configurableDataSource.Configure((IConfiguration)config), dataSourceSection);
        }
    }
}
