using Prism.Ioc;
using Prism.Regions;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Sample.Models;
using RPGDataEditor.Sample.Wpf.Views;
using RPGDataEditor.Wpf.Views;
using System;

namespace RPGDataEditor.Sample.Wpf
{
    public class TabModule : RegionTabModuleBase
    {
        public TabModule(IRegionManager regionManager) : base(regionManager) { }

        protected override Type[] GetTabTypes() => new Type[] {
                typeof(ConnectionTab),
                typeof(NpcTab),
                typeof(DialogueTab),
                typeof(QuestTab),
                typeof(SettingsTab)
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
