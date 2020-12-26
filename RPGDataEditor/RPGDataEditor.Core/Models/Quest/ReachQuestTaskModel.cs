namespace RPGDataEditor.Core.Models
{
    public class ReachQuestTaskModel : QuestTask
    {
        private Position pos;
        public Position Pos {
            get => pos;
            set => SetProperty(ref pos, value);
        }

        private double distance;
        public double Distance {
            get => distance;
            set => SetProperty(ref distance, value);
        }

        private bool reached;
        public bool Reached {
            get => reached;
            set => SetProperty(ref reached, value);
        }
    }
}
