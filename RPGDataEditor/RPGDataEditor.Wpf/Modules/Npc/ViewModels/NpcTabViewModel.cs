using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : SimpleIdentifiableTabViewModel<NpcDataModel>
    {
        public NpcTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "npcs";

        protected override async Task<EditorResults> OpenEditorAsync(SimpleIdentifiableData model)
        {
            EditorResults results = await base.OpenEditorAsync(model);
            if (results.Success)
            {
                Session.OnResourceChanged(RPGResource.Npc);
            }
            return results;
        }

        protected override async Task<bool> RemoveModelAsync(SimpleIdentifiableData model)
        {
            bool result = await base.RemoveModelAsync(model);
            Session.OnResourceChanged(RPGResource.Npc);
            return result;
        }
    }
}
