using FluentValidation;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Models.Quest>
    {
        public QuestEditorViewModel(IValidator<Models.Quest> validator, ILogger<QuestEditorViewModel> logger) : base(validator, logger) { }
    }
}
