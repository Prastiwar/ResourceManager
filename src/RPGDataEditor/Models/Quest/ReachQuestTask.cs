namespace RPGDataEditor.Models
{
    public class ReachQuestTask : IQuestTask
    {
        public Position Pos { get; set; }

        public double Distance { get; set; } = 1;
    }
}
