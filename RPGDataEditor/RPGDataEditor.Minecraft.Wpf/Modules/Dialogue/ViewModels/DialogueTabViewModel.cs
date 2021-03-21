using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : SimpleCategorizedTabViewModel<DialogueModel>
    {
        public DialogueTabViewModel(ViewModelContext context,
                                    ITypeToResourceConverter resourceConverter,
                                    ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter, simpleResourceConverter) { }

        protected override DialogueModel CreateNewExactModel(SimpleIdentifiableData model) => new DialogueModel() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as SimpleCategorizedData).Category
        };

        protected override async Task<EditorResults> OpenEditorAsync(SimpleIdentifiableData model)
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

        protected override async Task<bool> RemoveModelAsync(SimpleIdentifiableData model)
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
