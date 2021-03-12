using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : SimpleCategorizedTabViewModel<DialogueModel>
    {
        public DialogueTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "dialogues";

        protected override DialogueModel CreateNewExactModel(SimpleIdentifiableData model) => new DialogueModel() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as SimpleCategorizedData).Category
        };
    }
}
