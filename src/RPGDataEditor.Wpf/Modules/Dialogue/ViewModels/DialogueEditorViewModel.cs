using FluentValidation;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<Models.Dialogue>
    {
        public DialogueEditorViewModel(IValidator<Models.Dialogue> validator, ILogger<DialogueEditorViewModel> logger) : base(validator, logger) { }

        public override string Title => "Dialogue Editor";
    }
}
