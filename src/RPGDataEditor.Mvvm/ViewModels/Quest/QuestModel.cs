using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Mvvm.Models
{
    public class QuestModel : IdentifiableData
    {
        private IList<QuestTask> tasks = new ObservableCollection<QuestTask>();
        public IList<QuestTask> Tasks {
            get => tasks;
            set => SetProperty(ref tasks, value ?? new ObservableCollection<QuestTask>());
        }

        private QuestTask completionTask;
        public QuestTask CompletionTask {
            get => completionTask;
            set => SetProperty(ref completionTask, value);
        }
    }
}
