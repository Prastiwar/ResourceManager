using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Models.Npc>
    {
        public NpcTabViewModel(ViewModelContext context) : base(context) { }

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            EditorResults results = await base.OpenEditorAsync(model);
            if (results.Success)
            {
                Session.OnResourceChanged(RPGResource.Npc);
            }
            return results;
        }

        protected override async Task<bool> RemoveModelAsync(PresentableData model)
        {
            bool result = await base.RemoveModelAsync(model);
            Session.OnResourceChanged(RPGResource.Npc);
            return result;
        }
    }
}
