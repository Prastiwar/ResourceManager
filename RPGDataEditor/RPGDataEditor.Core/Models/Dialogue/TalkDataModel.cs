using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Models
{
    public class TalkDataModel : ObservableModel
    {
        private int talkRange = 10;
        public int TalkRange {
            get => talkRange;
            set => SetProperty(ref talkRange, value);
        }

        private IList<int> initationDialogues = new ObservableCollection<int>();
        public IList<int> InitationDialogues {
            get => initationDialogues;
            set => SetProperty(ref initationDialogues, value ?? new ObservableCollection<int>());
        }

        private IList<TalkLine> interactLines = new ObservableCollection<TalkLine>();
        public IList<TalkLine> InteractLines {
            get => interactLines;
            set => SetProperty(ref interactLines, value ?? new ObservableCollection<TalkLine>());
        }

        private IList<TalkLine> hurtLines = new ObservableCollection<TalkLine>();
        public IList<TalkLine> HurtLines {
            get => hurtLines;
            set => SetProperty(ref hurtLines, value ?? new ObservableCollection<TalkLine>());
        }

        private IList<TalkLine> deathLines = new ObservableCollection<TalkLine>();
        public IList<TalkLine> DeathLines {
            get => deathLines;
            set => SetProperty(ref deathLines, value ?? new ObservableCollection<TalkLine>());
        }
    }
}
