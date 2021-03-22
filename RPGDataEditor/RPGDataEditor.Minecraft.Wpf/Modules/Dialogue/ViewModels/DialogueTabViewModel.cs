using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewModels.DialogueTabViewModel
    {
        public DialogueTabViewModel(ViewModelContext context,
                                    ITypeToResourceConverter resourceConverter,
                                    ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter, simpleResourceConverter) { }

        protected override DialogueModel CreateNewExactModel(SimpleIdentifiableData model) => new Models.DialogueModel() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as SimpleCategorizedData).Category
        };
    }
}
