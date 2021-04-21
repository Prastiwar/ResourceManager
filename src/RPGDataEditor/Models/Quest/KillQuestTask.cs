namespace RPGDataEditor.Models
{
    public class KillQuestTask : QuestTask
    {
        public object TargetId { get; set; }

        public int Amount { get; set; }

        public int Counter { get; set; }
    }
}
