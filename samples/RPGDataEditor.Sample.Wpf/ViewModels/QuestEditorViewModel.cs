using FluentValidation;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Sample.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Quest>
    {
        public QuestEditorViewModel(IValidator<Quest> validator, ILogger<QuestEditorViewModel> logger) : base(validator, logger) { }
    }
}
