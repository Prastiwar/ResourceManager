using Prism.Ioc;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Minecraft.Wpf.Dialogue.Views;
using RPGDataEditor.Minecraft.Wpf.Npc.Views;
using RPGDataEditor.Wpf.Dialogue.ViewModels;
using RPGDataEditor.Wpf.Npc.ViewModels;

namespace RPGDataEditor.Minecraft.Wpf
{
    public class TabModule : RPGDataEditor.Wpf.TabModule
    {
        public TabModule(IRegionManager regionManager) : base(regionManager) { }

        protected override void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            base.RegisterDialogs(containerRegistry);
            containerRegistry.RegisterDialog<DialogueEditor, DialogueEditorViewModel>(typeof(DialogueModel).Name);
            containerRegistry.RegisterDialog<NpcDataModelEditor, NpcDataModelEditorViewModel>(typeof(NpcDataModel).Name);
        }
    }
}
