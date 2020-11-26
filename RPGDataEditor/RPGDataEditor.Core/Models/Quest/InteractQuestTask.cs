namespace RPGDataEditor.Core.Models
{
    public class InteractQuestTask : ObservableModel
    {
        private bool completed;
        public bool Completed {
            get => completed;
            set => SetProperty(ref completed, value);
        }
    }
}
