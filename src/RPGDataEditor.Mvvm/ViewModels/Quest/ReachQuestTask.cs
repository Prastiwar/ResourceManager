namespace RPGDataEditor.Mvvm.Models
{
    public class ReachQuestTask : QuestTask
    {
        private Position pos = new Position();
        public Position Pos {
            get => pos;
            set => SetProperty(ref pos, value ?? new Position());
        }

        private double distance = 1;
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
