namespace RPGDataEditor.Core.Models
{
    public class BlockInteractQuestTask : InteractQuestTask
    {
        private bool leftClick;
        public bool LeftClick {
            get => leftClick;
            set => SetProperty(ref leftClick, value);
        }

        private Position pos = new Position();
        public Position Pos {
            get => pos;
            set => SetProperty(ref pos, value ?? new Position());
        }
    }
}
