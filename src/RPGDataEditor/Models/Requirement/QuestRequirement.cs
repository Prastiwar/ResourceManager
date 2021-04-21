namespace RPGDataEditor.Models
{
    public class QuestRequirement : PlayerRequirementModel
    {
        public object QuestId { get; set; }

        public QuestStage Stage { get; set; }
    }
}
