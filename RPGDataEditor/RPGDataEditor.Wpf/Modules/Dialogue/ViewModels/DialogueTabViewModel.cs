using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : CategorizedTabViewModel<DialogueModel>
    {
        public DialogueTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "dialogues";
    }
}
