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
    }
}
