namespace RpgDataEditor.Models
{
    public class ReachQuestTask : QuestTask
    {
        public Position Pos { get; set; }

        public double Distance { get; set; } = 1;
    }
}
