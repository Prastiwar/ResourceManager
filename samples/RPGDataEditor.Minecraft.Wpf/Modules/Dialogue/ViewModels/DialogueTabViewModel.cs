using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewModels.DialogueTabViewModel
    {
        public DialogueTabViewModel(ViewModelContext context) : base(context) { }

        protected override RPGDataEditor.Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

    }
}
