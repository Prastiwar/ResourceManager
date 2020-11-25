using DryIoc;
using FluentValidation;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Views;
using RPGDataEditor.Wpf.Mvvm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf
{
    public partial class App
    {
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
                JsonSerializerSettings settings = new JsonSerializerSettings();
                // TODO: setup json settings
                return settings;
            };

            SessionContext session = new SessionContext();
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                string json = File.ReadAllText(SessionFilePath);
                session = JsonConvert.DeserializeObject<SessionContext>(json);
            }
            containerRegistry.RegisterInstance(session);

            IValidationProvider validatorProvider = new ValidatorProvider(Container);
            containerRegistry.RegisterInstance(validatorProvider);

            ViewModelContext context = new ViewModelContext(session, Container.Resolve<IDialogService>(), validatorProvider);
            containerRegistry.RegisterInstance(context);
        }

        protected void RegisterValidators(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidator<SessionContext>, SessionContextValidator>();
            ;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
