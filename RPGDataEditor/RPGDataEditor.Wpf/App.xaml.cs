using Prism.Ioc;
using RPGDataEditor.Views;
using System.Windows;
using Prism.Modularity;
using RPGDataEditor.Modules.ModuleName;
using RPGDataEditor.Services.Interfaces;
using RPGDataEditor.Services;

namespace RPGDataEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<QuestModule>();
        }
    }
}
