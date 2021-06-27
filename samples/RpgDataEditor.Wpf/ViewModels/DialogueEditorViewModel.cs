using FluentValidation;
using Microsoft.Extensions.Logging;
using ResourceManager.Mvvm;
using RpgDataEditor.Models;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<Dialogue>
    {
        public DialogueEditorViewModel(IValidator<Dialogue> validator, ILogger<DialogueEditorViewModel> logger) : base(validator, logger) { }

        public override string Title => "Dialogue Editor";
    }
}
