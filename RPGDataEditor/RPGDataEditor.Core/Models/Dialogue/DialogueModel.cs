using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Models
{
    public class DialogueModel : IdentifiableData
    {
        private int startQuest = -1;
        public int StartQuest {
            get => startQuest;
            set => SetProperty(ref startQuest, value);
        }

        private IList<DialogueOptionModel> options = new ObservableCollection<DialogueOptionModel>();
        public IList<DialogueOptionModel> Options {
            get => options;
            set => SetProperty(ref options, value ?? new ObservableCollection<DialogueOptionModel>());
        }
    }
}
