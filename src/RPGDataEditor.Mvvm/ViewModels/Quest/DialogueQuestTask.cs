namespace RPGDataEditor.Mvvm.Models
{
    public class DialogueQuestTask : InteractQuestTask
    {
        private int dialogueId = -1;
        public int DialogueId {
            get => dialogueId;
            set => SetProperty(ref dialogueId, value);
        }
    }
}
