using Prism.Ioc;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Connection.Views;
using RPGDataEditor.Wpf.Dialogue.Views;
using RPGDataEditor.Wpf.Npc.Views;
using RPGDataEditor.Wpf.Quest.Views;
using RPGDataEditor.Wpf.Settings.Views;
using RPGDataEditor.Wpf.Views;
using System;

namespace RPGDataEditor.Wpf
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
            containerRegistry.RegisterDialog<DialogueEditor>(typeof(DialogueModel).Name);
            containerRegistry.RegisterDialog<NpcDataModelEditor>(typeof(NpcDataModel).Name);
            containerRegistry.RegisterDialog<QuestEditor>(typeof(QuestModel).Name);
            containerRegistry.RegisterDialog<PickerDialog>(typeof(PickerDialog).Name);
        }

    }
}
