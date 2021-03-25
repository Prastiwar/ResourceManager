namespace RPGDataEditor.Core.Models
{
    public class BlockInteractQuestTask : InteractQuestTask
    {
        private Position pos = new Position();
        public Position Pos {
            get => pos;
            set => SetProperty(ref pos, value ?? new Position());
        }
    }
}
