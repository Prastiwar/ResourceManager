using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : PresentableCategoryDataViewModel<Models.Quest>
    {
        public QuestTabViewModel(ViewModelContext context) : base(context) { }

        protected override Models.Quest CreateResource(PresentableData model) => new Models.Quest() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            EditorResults results = await base.OpenEditorAsync(model);
            if (results.Success)
            {
                Session.OnResourceChanged(RPGResource.Quest);
            }
            return results;
        }

        public override async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            bool renamed = await base.RenameCategoryAsync(oldCategory, newCategory);
            if (renamed)
            {
                Session.OnResourceChanged(RPGResource.Quest);
            }
            return renamed;
        }

        protected override async Task<bool> RemoveModelAsync(PresentableData model)
        {
            bool result = await base.RemoveModelAsync(model);
            Session.OnResourceChanged(RPGResource.Quest);
            return result;
        }

        public override async Task<bool> RemoveCategoryAsync(string category)
        {
            bool result = await base.RemoveCategoryAsync(category);
            Session.OnResourceChanged(RPGResource.Quest);
            return result;
        }
    }
}
