namespace RPGDataEditor.Core.Models
{
    public class TalkDataModel : ObservableModel
    {
        private int talkRange = 10;
        public int TalkRange {
            get => talkRange;
            set => SetProperty(ref talkRange, value);
        }

        private int[] initationDialogues = new int[0];
        public int[] InitationDialogues {
            get => initationDialogues;
            set => SetProperty(ref initationDialogues, value ?? new int[0]);
        }

        private TalkLine[] interactLines = new TalkLine[0];
        public TalkLine[] InteractLines {
            get => interactLines;
            set => SetProperty(ref interactLines, value ?? new TalkLine[0]);
        }

        private TalkLine[] hurtLines = new TalkLine[0];
        public TalkLine[] HurtLines {
            get => hurtLines;
            set => SetProperty(ref hurtLines, value ?? new TalkLine[0]);
        }

        private TalkLine[] deathLines = new TalkLine[0];
        public TalkLine[] DeathLines {
            get => deathLines;
            set => SetProperty(ref deathLines, value ?? new TalkLine[0]);
        }
    }
}
