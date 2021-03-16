using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<DialogueModel>
    {
        public DialogueEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Dialogue Editor";

        public ICommand AddOptionCommand => Commands.AddListItemLiCommand(() => Model.Options);

        public ICommand RemoveOptionCommand => Commands.RemoveListItemLiCommand(() => Model.Options);

        public ICommand AddRequirementCommand => Commands.AddListItemLiCommand(() => Model.Requirements, () => new DialogueRequirement());

        public ICommand RemoveRequirementCommand => Commands.RemoveListItemLiCommand(() => Model.Requirements);
    }
}
