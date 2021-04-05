namespace RPGDataEditor.Core.Models
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
