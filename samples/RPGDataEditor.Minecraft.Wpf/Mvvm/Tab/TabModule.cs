using Prism.Ioc;
using Prism.Regions;
using RPGDataEditor.Minecraft.Wpf.Dialogue.Views;
using RPGDataEditor.Minecraft.Wpf.Npc.ViewModels;
using RPGDataEditor.Minecraft.Wpf.Npc.Views;
using RPGDataEditor.Wpf.Dialogue.ViewModels;

namespace RPGDataEditor.Minecraft.Wpf
{
    public class TabModule : RPGDataEditor.Wpf.TabModule
    {
        public TabModule(IRegionManager regionManager) : base(regionManager) { }

        protected override void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            base.RegisterDialogs(containerRegistry);
            containerRegistry.RegisterDialog<DialogueEditor, DialogueEditorViewModel>(typeof(RPGDataEditor.Models.Dialogue).Name);
            containerRegistry.RegisterDialog<NpcEditor, NpcEditorViewModel>(typeof(RPGDataEditor.Models.Npc).Name);
        }
    }
}
