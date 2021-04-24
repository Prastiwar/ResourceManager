using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : PresentableCategoryDataViewModel<Models.Dialogue>
    {
        public DialogueTabViewModel(ViewModelContext context) : base(context) { }

        protected override Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
