using DryIoc;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
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
            JsonConvert.DefaultSettings = () => {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                // TODO: setup json settings
                return settings;
            };

            SessionContext context = new SessionContext();
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            if (sessionFile.Exists)
            {
                string json = File.ReadAllText(SessionFilePath);
                context = JsonConvert.DeserializeObject<SessionContext>(json);
            }
            containerRegistry.RegisterInstance(context);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();
    }
}
