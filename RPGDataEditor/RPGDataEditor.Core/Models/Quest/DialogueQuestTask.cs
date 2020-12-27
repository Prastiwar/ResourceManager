namespace RPGDataEditor.Core.Models
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
