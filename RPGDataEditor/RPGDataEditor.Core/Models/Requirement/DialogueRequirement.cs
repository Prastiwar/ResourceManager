namespace RPGDataEditor.Core.Models
{
    public class DialogueRequirement : PlayerRequirementModel
    {
        private int dialogueId;
        public int DialogueId {
            get => dialogueId;
            set => SetProperty(ref dialogueId, value);
        }

        private bool completed;
        public bool Completed {
            get => completed;
            set => SetProperty(ref completed, value);
        }
    }
}
