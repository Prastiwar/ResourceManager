namespace RPGDataEditor.Models
{
    public class DialogueRequirement : PlayerRequirementModel
    {
        public object DialogueId { get; set; }

        public bool Completed { get; set; }
    }
}
