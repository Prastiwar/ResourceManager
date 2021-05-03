using RPGDataEditor.Core;
using RPGDataEditor.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewRPGDataEditor.Models.DialogueTabViewModel
    {
        public DialogueTabViewModel(ViewModelContext context,
                                    ITypeToResourceConverter resourceConverter,
                                    ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter, simpleResourceConverter) { }

        protected override Dialogue CreateNewExactModel(SimpleIdentifiableData model) => new RPGDataEditor.Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as SimpleCategorizedData).Category
        };
    }
}
