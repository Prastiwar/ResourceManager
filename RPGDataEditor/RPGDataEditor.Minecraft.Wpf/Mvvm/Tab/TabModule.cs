using Prism.Ioc;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Minecraft.Wpf.Dialogue.Views;
using RPGDataEditor.Minecraft.Wpf.Npc.Views;

namespace RPGDataEditor.Minecraft.Wpf
{
    public class TabModule : RPGDataEditor.Wpf.TabModule
    {
        public TabModule(IRegionManager regionManager) : base(regionManager) { }

        protected override void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            base.RegisterDialogs(containerRegistry);
            containerRegistry.RegisterDialog<DialogueEditor>(typeof(DialogueModel).Name);
            containerRegistry.RegisterDialog<NpcDataModelEditor>(typeof(NpcDataModel).Name);
        }

    }
}
