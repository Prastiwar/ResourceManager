namespace RPGDataEditor.Core.Models
{
    public class KillQuestTaskModel : ObservableModel
    {
        private string kill;
        public string Kill {
            get => kill;
            set => SetProperty(ref kill, value);
        }

        private int amount;
        public int Amount {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        private int counter;
        public int Counter {
            get => amount;
            set => SetProperty(ref counter, value);
        }
    }
}
