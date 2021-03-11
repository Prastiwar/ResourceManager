namespace RPGDataEditor.Core.Models
{
    public class KillQuestTask : QuestTask
    {
        private string kill;
        public string Kill {
            get => kill;
            set => SetProperty(ref kill, value);
        }

        private int amount = 1;
        public int Amount {
            get => amount;
            set => SetProperty(ref amount, value < 1 ? 1 : value);
        }

        private int counter;
        public int Counter {
            get => amount;
            set => SetProperty(ref counter, value);
        }
    }
}
