using MediatR;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Models.Quest>
    {
        public QuestEditorViewModel(IMediator mediator, ILogger<QuestEditorViewModel> logger) : base(mediator, logger) { }

        //public string QuestTitle {
        //    get => Model.Title;
        //    set => SetProperty(QuestTitle, value, () => Model.Title = value);
        //}

        //public string Message {
        //    get => Model.Message;
        //    set => SetProperty(Message, value, () => Model.Message = value);
        //}

        //public ObservableModel CompletionTask {
        //    get => Mapper.Map<ObservableModel>(Model.CompletionTask);
        //    set => SetProperty(CompletionTask, value, () => Model.CompletionTask = Mapper.Map<IQuestTask>(value));
        //}

        //public IList<ObservableModel> Tasks {
        //    get => Mapper.Map<IList<ObservableModel>>(Model.Tasks);
        //    set => SetProperty(Tasks, value, () => Model.Tasks = Mapper.Map<IList<IQuestTask>>(value));
        //}

        //public IList<ObservableModel> Requirements {
        //    get => Mapper.Map<IList<ObservableModel>>(Model.Requirements);
        //    set => SetProperty(Requirements, value, () => Model.Requirements = Mapper.Map<IList<Requirement>>(value));
        //}
    }
}
