using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Connection.Views;
using RPGDataEditor.Wpf.Dialogue.Views;
using RPGDataEditor.Wpf.Npc.Views;
using RPGDataEditor.Wpf.Quest.Views;
using RPGDataEditor.Wpf.Settings.Views;
using RPGDataEditor.Wpf.Views;

namespace RPGDataEditor.Wpf
{
    public class TabModule : IModule
    {
        private readonly IRegionManager regionManager;

        public TabModule(IRegionManager regionManager) => this.regionManager = regionManager;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ConnectionTab));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(NpcTab));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DialogueTab));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(QuestTab));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(SettingsTab));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConnectionTab>();
            containerRegistry.RegisterForNavigation<NpcTab>();
            containerRegistry.RegisterForNavigation<DialogueTab>();
            containerRegistry.RegisterForNavigation<QuestTab>();
            containerRegistry.RegisterForNavigation<SettingsTab>();
            containerRegistry.RegisterDialog<DialogueEditor>(typeof(DialogueModel).Name);
            containerRegistry.RegisterDialog<NpcDataModelEditor>(typeof(NpcDataModel).Name);
            containerRegistry.RegisterDialog<QuestEditor>(typeof(QuestModel).Name);
            containerRegistry.RegisterDialog<PickerDialog>(nameof(PickerDialog));
        }
    }
}