using FluentValidation;
using Microsoft.Extensions.Logging;
using RpgDataEditor.Models;
using ResourceManager.Mvvm;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Quest>
    {
        public QuestEditorViewModel(IValidator<Quest> validator, ILogger<QuestEditorViewModel> logger) : base(validator, logger) { }
    }
}
