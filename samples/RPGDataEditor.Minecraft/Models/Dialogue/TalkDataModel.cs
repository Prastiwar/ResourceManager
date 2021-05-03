using RPGDataEditor.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Models
{
    public class TalkData : RPGDataEditor.Models.TalkData
    {
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
