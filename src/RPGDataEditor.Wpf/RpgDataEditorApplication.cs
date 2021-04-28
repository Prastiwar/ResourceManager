using AutoUpdaterDotNET;
using DryIoc;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ResourceManager;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Extensions.Prism.Wpf.Services;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Providers;
using RPGDataEditor.Services;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Providers;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

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

        protected virtual string SessionFilePath => Path.Combine(CacheDirectoryPath, "session.json");

        //public ISessionContext Session { get; private set; }

        protected virtual void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
            => Container.Resolve<ILogger>().Error("Unhandled exception", e.Exception);

        protected virtual void OnExit(object sender, ExitEventArgs e) { }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlAdapter>());
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
            settings.Converters.Add(new QuestDataJsonConverter());

            settings.Converters.Add(new DialogueJsonConverter());
            settings.Converters.Add(new DialogueOptionJsonConverter());
            settings.Converters.Add(new TalkDataModelJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());


            settings.Converters.Add(new FileClientJsonConverter());
            return settings;
        }

        protected virtual void LoadSession()
        {
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                IMemoryCache cache = Container.Resolve<IMemoryCache>();
                string json = File.ReadAllText(SessionFilePath);
                JObject obj = JsonConvert.DeserializeObject<JObject>(json);
                foreach (System.Collections.Generic.KeyValuePair<string, JToken> item in obj)
                {
                    cache.Set(item.Key, item.Value.ToObject(typeof(object)));
                }
            }
        }

        protected virtual ViewModelContext CreateViewModelContext() => new ViewModelContext(Container.Resolve<IMediator>(),
                                                                                            Container.Resolve<RPGDataEditor.Mvvm.Services.IDialogService>(),
                                                                                            Container.Resolve<ILogger>());

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            JsonConvert.DefaultSettings = CreateJsonSettings;

            RegisterCache(containerRegistry);
            RegisterLogging(containerRegistry);
            try
            {
                LoadSession();
            }
            catch (Exception ex)
            {
                containerRegistry.GetContainer().Resolve<ILogger>().Error("Couldn't load session", ex);
                throw;
            }

            RegisterValidators(containerRegistry);
            RegisterProviders(containerRegistry);
            RegisterServices(containerRegistry);
            RegisterDialogs(containerRegistry);
            RegisterDescriptors(containerRegistry);

            InitializeAutoUpdater();

            containerRegistry.RegisterInstance(CreateViewModelContext());
            OnRegistrationFinished(containerRegistry);
        }

        protected virtual void RegisterCache(IContainerRegistry containerRegistry)
        {
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            containerRegistry.RegisterInstance<IMemoryCache>(cache);
        }

        protected virtual void RegisterLogging(IContainerRegistry containerRegistry) => containerRegistry.RegisterInstance<ILogger>(new LocalFileLogger());

        protected virtual void OnRegistrationFinished(IContainerRegistry containerRegistry) { }

        protected virtual void RegisterValidators(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidator<Models.Npc>, NpcValidator>();
            containerRegistry.Register<IValidator<Models.Quest>, QuestValidator>();
            containerRegistry.Register<IValidator<Models.Dialogue>, DialogueValidator>();
        }

        protected virtual void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ISnackbarService>(new SnackbarService());
            containerRegistry.RegisterSingleton<IDialogService, PrismDialogService>();
        }

        protected virtual void RegisterProviders(IContainerRegistry containerRegistry)
        {
            AssemblyName[] referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            Assembly[] assembliesToScan = new Assembly[referencedAssemblies.Length + 1];
            assembliesToScan[0] = Assembly.GetEntryAssembly();
            referencedAssemblies.CopyTo(assembliesToScan, 1);
            containerRegistry.RegisterInstance<IFluentAssemblyScanner>(new FluentAssemblyScanner(assembliesToScan));
            containerRegistry.RegisterSingleton(typeof(IImplementationProvider<>), typeof(DefaultImplementationProvider<>));
            AutoTemplateProvider controlProvider = new AutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
        }

        protected virtual void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<UpdateDialog>(typeof(UpdateDialog).Name);
            containerRegistry.RegisterDialog<ConnectionDialog>(typeof(ConnectionDialog).Name);
        }

        protected virtual void RegisterDescriptors(IContainerRegistry containerRegistry)
        {
            // TODO: Register descriptors
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
