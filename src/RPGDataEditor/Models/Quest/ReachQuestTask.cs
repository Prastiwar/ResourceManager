namespace RPGDataEditor.Models
{
    public class ReachQuestTask : QuestTask
    {
        public Position Pos { get; set; }

        public double Distance { get; set; } = 1;

        public bool Reached { get; set; }
    }
}
