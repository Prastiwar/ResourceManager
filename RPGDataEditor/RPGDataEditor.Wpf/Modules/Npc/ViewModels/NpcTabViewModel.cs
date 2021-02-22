using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : IdentifiableTabViewModel<NpcDataModel>
    {
        public NpcTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "npcs";

        protected override string GetRelativeFilePath(NpcDataModel model) => RelativePath + $"/{model.Id}_{model.Name}.json";
    }
}
