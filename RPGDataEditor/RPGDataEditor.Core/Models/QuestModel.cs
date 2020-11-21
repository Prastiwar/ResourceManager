namespace RPGDataEditor.Core.Models
{
    public class QuestModel : IdentifiableData
    {
        private QuestTask[] tasks = new QuestTask[0];
        public QuestTask[] Tasks {
            get => tasks;
            set => SetProperty(ref tasks, value ?? new QuestTask[0]);
        }

        private QuestTask completionTask;
        public QuestTask CompletionTask {
            get => completionTask;
            set => SetProperty(ref completionTask, value);
        }
    }
}
