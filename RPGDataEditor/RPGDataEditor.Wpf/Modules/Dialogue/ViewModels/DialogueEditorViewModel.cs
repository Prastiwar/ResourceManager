using Prism.Commands;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<DialogueModel>
    {
        public DialogueEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Dialogue Editor";

        private ICommand addOptionCommand;
        public ICommand AddOptionCommand => addOptionCommand ??= new DelegateCommand(AddOption);

        private void AddOption()
        {
            DialogueOptionModel[] options = Model.Options;
            DialogueOptionModel[] newOptions = new DialogueOptionModel[options.Length + 1];
            newOptions[options.Length] = new DialogueOptionModel();
            Model.Options = newOptions;
        }
    }
}
