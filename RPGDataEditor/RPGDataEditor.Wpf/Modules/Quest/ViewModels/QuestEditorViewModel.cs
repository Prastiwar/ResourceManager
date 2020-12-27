using Prism.Services.Dialogs;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<QuestModel>
    {
        public QuestEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Quest Editor";

        public ICommand AddTaskCommand => Commands.AddListItemLiCommand(() => Model.Tasks, () => new KillQuestTask());

        public ICommand RemoveTaskCommand => Commands.RemoveListItemLiCommand(() => Model.Tasks);

        public ICommand AddRequirementCommand => Commands.AddListItemLiCommand(() => Model.Requirements, () => new DialogueRequirement());

        public ICommand RemoveRequirementCommand => Commands.RemoveListItemLiCommand(() => Model.Requirements);

        public int TasksCount => Model == null ? 0 : Model.Tasks.Count;

        public int RequirementsCount => Model == null ? 0 : Model.Requirements.Count;

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            base.InitializeAsync(parameters);
            if (Model.Tasks is INotifyCollectionChanged tasksNotifier)
            {
                tasksNotifier.CollectionChanged += Requirements_CollectionChanged;
            }
            if (Model.Requirements is INotifyCollectionChanged requirementsNotifier)
            {
                requirementsNotifier.CollectionChanged += Requirements_CollectionChanged;
            }
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Model))
            {
                RaisePropertyChanged(nameof(TasksCount));
                RaisePropertyChanged(nameof(RequirementsCount));
            }
        }

        private void Requirements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(RequirementsCount));

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(TasksCount));
    }
}
