using MediatR;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Models.Quest>
    {
        public QuestEditorViewModel(IMediator mediator, ILogger<QuestEditorViewModel> logger) : base(mediator, logger) { }
    }
}
