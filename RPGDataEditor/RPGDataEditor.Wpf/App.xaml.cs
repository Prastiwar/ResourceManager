﻿using DryIoc;
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
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Views;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf
{
    public partial class App
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Exit += App_Exit;
        }

        private static string AppRootPath => new DirectoryInfo(BinaryPath).Parent.FullName;
        public static string BinaryPath => Path.Combine(Environment.CurrentDirectory + "bin");
        public static string CacheDirectoryPath {
            get {
                string path = Path.Combine(AppRootPath, "cache");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public static string SessionFilePath => Path.Combine(CacheDirectoryPath, "session.json");

        public static SessionContext CurrentSession { get; private set; }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) => Logger.Error("Unhandled exception", e.Exception);

        private void App_Exit(object sender, ExitEventArgs e)
        {
            // TODO: Invoke on connection lost
        }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlAdapter>());
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterValidators(containerRegistry);

            JsonConvert.DefaultSettings = () => {
                PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
                propResolver.SetAllLetterCase(Lettercase.CamelCase);
                propResolver.IgnoreProperty(typeof(IdentifiableData), nameof(IdentifiableData.RepresentableString));
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
                return settings;
            };

            SessionContext session = new SessionContext();
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                string json = File.ReadAllText(SessionFilePath);
                session = JsonConvert.DeserializeObject<SessionContext>(json);
            }
            CurrentSession = session;

            containerRegistry.RegisterInstance(session);

            IValidationProvider validatorProvider = new ValidatorProvider(Container);
            containerRegistry.RegisterInstance(validatorProvider);

            ISnackbarService snackbarService = new SnackbarService();
            containerRegistry.RegisterInstance(snackbarService);

            ViewModelContext context = new ViewModelContext(session, Container.Resolve<IDialogService>(), validatorProvider, snackbarService);
            containerRegistry.RegisterInstance(context);

            containerRegistry.RegisterInstance<IAppLifetimeService>(new AppLifetimeService());

            AppVersionChecker versionChecker = new AppVersionChecker() {
                VersionPath = "https://raw.githubusercontent.com/Prastiwar/RPGDataEditor/main/version.json",
                ActualVersion = new AppVersion("1.0.0")
            };
            containerRegistry.RegisterInstance(versionChecker);

            containerRegistry.RegisterDialog<UpdateDialog>(nameof(UpdateDialog));
        }

        protected void RegisterValidators(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidator<SessionContext>, SessionContextValidator>();
            containerRegistry.Register<IValidator<NpcDataModel>, NpcDataModelValidator>();
            containerRegistry.Register<IValidator<QuestModel>, QuestModelValidator>();
            containerRegistry.Register<IValidator<DialogueModel>, DialogueModelValidator>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
