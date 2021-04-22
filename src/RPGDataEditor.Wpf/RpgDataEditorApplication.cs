﻿using DryIoc;
using FluentValidation;
using Newtonsoft.Json;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Providers;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MediatR;
using RPGDataEditor.Services;

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

        public ISessionContext Session { get; private set; }

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
            propResolver.IgnoreProperty(typeof(ISessionContext), nameof(ISessionContext.ClientProvider));
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
            settings.Converters.Add(new DialogueOptionModelJsonConverter());
            settings.Converters.Add(new TalkDataModelJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());


            settings.Converters.Add(new ResourceClientJsonConverter());
            settings.Converters.Add(new OptionsDataJsonConverter());
            settings.Converters.Add(new SessionContextJsonConverter());
            if (Session is DefaultSessionContext context)
            {
                settings.Formatting = context.Options.PrettyPrint ? Formatting.Indented : Formatting.None;
            }
            return settings;
        }

        protected virtual ISessionContext CreateSession()
        {
            ISessionContext session = new DefaultSessionContext(SessionFilePath);
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                session = session.LoadSession();
            }
            return session;
        }

        protected virtual ViewModelContext CreateViewModelContext() => new ViewModelContext(Container.Resolve<IMediator>(),
                                                                                            Container.Resolve<IDialogService>(),
                                                                                            Container.Resolve<ILogger>());

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
            RegisterProviders(containerRegistry);
            RegisterServices(containerRegistry);
            RegisterDialogs(containerRegistry);
            RegisterConverters(containerRegistry);

            containerRegistry.RegisterInstance(CreateVersionChecker());

            containerRegistry.RegisterInstance(CreateViewModelContext());

            OnRegistrationFinished(containerRegistry);
        }

        protected virtual void OnRegistrationFinished(IContainerRegistry containerRegistry)
        {
            Session.ClientProvider = Container.Resolve<IClientProvider>();
            if (Session.Client == null)
            {
                Session.SetConnection("Local");
            }
        }

        protected virtual void RegisterValidators(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidator<ISessionContext>, SessionContextValidator>();
            containerRegistry.Register<IValidator<Models.Npc>, NpcValidator>();
            containerRegistry.Register<IValidator<Models.Quest>, QuestValidator>();
            containerRegistry.Register<IValidator<Models.Dialogue>, DialogueValidator>();
        }

        protected virtual void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ISnackbarService>(new SnackbarService());
            containerRegistry.RegisterInstance<IConnectionService>(new ConnectionService(Session, containerRegistry.GetContainer().Resolve<IConnectionCheckerProvider>()));
        }

        protected virtual void RegisterProviders(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IValidationProvider>(new ValidatorProvider(Container));
            containerRegistry.RegisterInstance<IModelProvider<PlayerRequirementModel>>(new DefaultRequirementProvider());
            containerRegistry.RegisterInstance<IModelProvider<QuestTask>>(new DefaultQuestTaskProvider());
            containerRegistry.RegisterInstance<INamedIdProvider<DialogueOptionModel>>(new DefaultDialogueOptionNamedIdProvider());
            containerRegistry.Register(typeof(IModelProvider<>), typeof(DefaultModelProvider<>));
            containerRegistry.Register<IClientProvider, DefaultClientProvider>();
            AutoTemplateProvider controlProvider = new AutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
            containerRegistry.RegisterInstance<IConnectionCheckerProvider>(new DefaultConnectionCheckerProvider());
        }

        protected virtual void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<UpdateDialog>(typeof(UpdateDialog).Name);
            containerRegistry.RegisterDialog<ConnectionDialog>(typeof(ConnectionDialog).Name);
        }

        protected virtual void RegisterConverters(IContainerRegistry containerRegistry)
        {
            DefaultResourceToPathConverter resourceToPathConverter = new DefaultResourceToPathConverter();
            containerRegistry.RegisterInstance<IResourceToPathConverter>(resourceToPathConverter);
            DefaultResourceToTypeConverter typeConverter = new DefaultResourceToTypeConverter();
            containerRegistry.RegisterInstance<IResourceToTypeConverter>(typeConverter);
            containerRegistry.RegisterInstance<ITypeToResourceConverter>(new DefaultTypeToResourceConverter());
            containerRegistry.RegisterInstance<ILocationToSimpleResourceConverter>(new DefaultLocationToSimpleResourceConverter(resourceToPathConverter, typeConverter));
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
