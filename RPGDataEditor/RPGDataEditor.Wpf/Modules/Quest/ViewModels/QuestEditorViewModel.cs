using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<QuestModel>
    {
        public QuestEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Quest Editor";

        public ICommand AddTaskCommand => Commands.AddListItemCommand(() => Model.Tasks, () => new KillQuestTask());

        public ICommand RemoveTaskCommand => Commands.RemoveListItemLiCommand(() => Model.Tasks);

        public ICommand AddRequirementCommand => Commands.AddListItemCommand(() => Model.Requirements, () => new DialogueRequirement());

        public ICommand RemoveRequirementCommand => Commands.RemoveListItemLiCommand(() => Model.Requirements);
    }
}
