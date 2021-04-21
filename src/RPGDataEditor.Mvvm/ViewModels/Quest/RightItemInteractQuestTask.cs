namespace RPGDataEditor.Mvvm.Models
{
    public class RightItemInteractQuestTask : InteractQuestTask
    {
        private string item;
        public string Item {
            get => item;
            set => SetProperty(ref item, value);
        }
    }
}
