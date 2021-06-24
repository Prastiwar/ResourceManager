namespace RPGDataEditor.Sample.Models
{
    public class KillQuestTask : IQuestTask
    {
        public object TargetId { get; set; }

        public int Amount { get; set; }
    }
}
