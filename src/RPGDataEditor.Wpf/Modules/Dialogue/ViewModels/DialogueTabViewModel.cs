using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : PresentableCategoryDataViewModel<Models.Dialogue>
    {
        public DialogueTabViewModel(ViewModelContext context)            : base(context) { }

        protected override Models.Dialogue CreateResource(PresentableData model) => new DModels.ialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            EditorResults results = await base.OpenEditorAsync(model);
            if (results.Success)
            {
                Session.OnResourceChanged(RPGResource.Dialogue);
            }
            return results;
        }

        public override async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            bool renamed = await base.RenameCategoryAsync(oldCategory, newCategory);
            Session.OnResourceChanged(RPGResource.Dialogue);
            return renamed;
        }

        protected override async Task<bool> RemoveModelAsync(PresentableData model)
        {
            bool result = await base.RemoveModelAsync(model);
            Session.OnResourceChanged(RPGResource.Dialogue);
            return result;
        }

        public override async Task<bool> RemoveCategoryAsync(string category)
        {
            bool result = await base.RemoveCategoryAsync(category);
            Session.OnResourceChanged(RPGResource.Dialogue);
            return result;
        }
    }
}
