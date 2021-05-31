using MediatR;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Collections.Generic;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Models.Quest>
    {
        public QuestEditorViewModel(IMediator mediator, ILogger<QuestEditorViewModel> logger) : base(mediator, logger) { }

        public override string Title => "Quest Editor";

        public string QuestTitle {
            get => Model.Title;
            set => SetProperty(QuestTitle, value, () => Model.Title = value);
        }

        public string Message {
            get => Model.Message;
            set => SetProperty(Message, value, () => Model.Message = value);
        }

        public IQuestTask CompletionTask {
            get => Model.CompletionTask;
            set => SetProperty(CompletionTask, value, () => Model.CompletionTask = value);
        }

        public IList<IQuestTask> Tasks {
            get => Model.Tasks;
            set => SetProperty(Tasks, value, () => Model.Tasks = value);
        }

        public IList<Requirement> Requirements {
            get => Model.Requirements;
            set => SetProperty(Requirements, value, () => Model.Requirements = value);
        }
    }
}
