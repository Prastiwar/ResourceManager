using Prism.Ioc;
using Prism.Regions;
using ResourceManager.Mvvm;
using RpgDataEditor.Models;
using RpgDataEditor.Wpf.Views;
using ResourceManager.Wpf.Views;
using System;

namespace RpgDataEditor.Wpf
{
    public class TabModule : RegionTabModuleBase
    {
        public TabModule(IRegionManager regionManager) : base(regionManager) { }

        protected override Type[] GetTabTypes() => new Type[] {
                typeof(ConnectionTab),
                typeof(NpcTab),
                typeof(DialogueTab),
                typeof(QuestTab),
                typeof(BackupTab)
            };

        protected override void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<DialogueEditor>(typeof(Dialogue).Name);
            containerRegistry.RegisterDialog<NpcEditor>(typeof(Npc).Name);
            containerRegistry.RegisterDialog<QuestEditor>(typeof(Quest).Name);
            containerRegistry.RegisterDialog<PickerDialog>(typeof(PickerDialog).Name);
        }

    }
}
