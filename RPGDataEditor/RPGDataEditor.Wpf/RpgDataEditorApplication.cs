using DryIoc;
using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Views;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using RPGDataEditor.Core.Connection;
using Prism.DryIoc;

namespace RPGDataEditor.Wpf
{
    public class RpgDataEditorApplication : PrismApplication
    {
        public RpgDataEditorApplication()
        {
            DispatcherUnhandledException += OnUnhandledException;
            Exit += OnExit;
        }

        public static new RPGDataEditorApp Current => Application.Current as RPGDataEditorApp;

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

        public SessionContext Session { get; private set; }

        protected virtual void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
            => Logger.Error("Unhandled exception", e.Exception);

        protected virtual async void OnExit(object sender, ExitEventArgs e) => await Session.Client.DisconnectAsync();

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
            propResolver.IgnoreProperty(typeof(IdentifiableData), nameof(IdentifiableData.RepresentableString));
            propResolver.IgnoreProperty(typeof(SessionContext), nameof(SessionContext.ClientProvider));
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new PlayerRequirementJsonConverter());

            settings.Converters.Add(new NpcJobJsonConverter());
            settings.Converters.Add(new EquipmentModelJsonConverter());
            settings.Converters.Add(new NpcDataModelJsonConverter());
            settings.Converters.Add(new TradeItemModelJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new QuestTaskJsonConverter());
            settings.Converters.Add(new QuestDataJsonConverter());

            settings.Converters.Add(new DialogueModelJsonConverter());
            settings.Converters.Add(new DialogueOptionModelJsonConverter());
            settings.Converters.Add(new TalkDataModelJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());

            settings.Converters.Add(new ConnectionJsonConverter());
            return settings;
        }

        protected virtual SessionContext CreateSession()
        {
            SessionContext session = new SessionContext(SessionFilePath);
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                session = session.LoadSession();
            }
            return session;
        }

        protected virtual ViewModelContext CreateViewModelContext() => new ViewModelContext(Session,
                                                                                            Container.Resolve<IDialogService>(),
                                                                                            Container.Resolve<IValidationProvider>(),
                                                                                            Container.Resolve<ISnackbarService>());

        protected virtual AppVersionChecker CreateVersionChecker() => new AppVersionChecker() {
            VersionPath = "https://raw.githubusercontent.com/Prastiwar/RPGDataEditor/main/version.json",
            ActualVersion = new AppVersion("1.0.0")
        };

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            JsonConvert.DefaultSettings = CreateJsonSettings;

            try
            {
                Session = CreateSession();
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't load session", ex);
                throw;
            }
            containerRegistry.RegisterInstance(Session);

            RegisterValidators(containerRegistry);
            RegisterServices(containerRegistry);
            RegisterProviders(containerRegistry);
            RegisterDialogs(containerRegistry);
            RegisterConverters(containerRegistry);

            containerRegistry.RegisterInstance(CreateVersionChecker());

            containerRegistry.RegisterInstance(CreateViewModelContext());

            OnRegistrationFinished(containerRegistry);
        }
        protected virtual void OnRegistrationFinished(IContainerRegistry containerRegistry)
            => Session.ClientProvider = Container.Resolve<IClientProvider>();

        protected virtual void RegisterValidators(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidator<SessionContext>, SessionContextValidator>();
            containerRegistry.Register<IValidator<NpcDataModel>, NpcDataModelValidator>();
            containerRegistry.Register<IValidator<QuestModel>, QuestModelValidator>();
            containerRegistry.Register<IValidator<DialogueModel>, DialogueModelValidator>();
        }

        protected virtual void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ISnackbarService>(new SnackbarService());
            containerRegistry.RegisterInstance<IAppLifetimeService>(new AppLifetimeService());
        }

        protected virtual void RegisterProviders(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IValidationProvider>(new ValidatorProvider(Container));
            containerRegistry.RegisterInstance<IRequirementProvider>(new DefaultRequirementProvider());
            containerRegistry.Register<IClientProvider, DefaultClientProvider>();
        }

        protected virtual void RegisterDialogs(IContainerRegistry containerRegistry)
            => containerRegistry.RegisterDialog<UpdateDialog>(nameof(UpdateDialog));

        protected virtual void RegisterConverters(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IResourceToPathConverter>(new DefaultResourceToPathConverter());
            containerRegistry.RegisterInstance<IResourceToTypeConverter>(new DefaultResourceToTypeConverter());
            containerRegistry.RegisterInstance<ITypeToResourceConverter>(new DefaultTypeToResourceConverter());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
