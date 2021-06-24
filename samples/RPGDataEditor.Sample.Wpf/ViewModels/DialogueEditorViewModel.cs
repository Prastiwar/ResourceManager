using FluentValidation;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<Dialogue>
    {
        public DialogueEditorViewModel(IValidator<Dialogue> validator, ILogger<DialogueEditorViewModel> logger) : base(validator, logger) { }

        public override string Title => "Dialogue Editor";
    }
}
