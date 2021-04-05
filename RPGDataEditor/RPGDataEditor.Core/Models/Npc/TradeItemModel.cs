namespace RPGDataEditor.Core.Models
{
    public class TradeItemModel : ObservableModel
    {
        private string item;
        public string Item {
            get => item;
            set => SetProperty(ref item, value);
        }

        private int buy;
        public int Buy {
            get => buy;
            set => SetProperty(ref buy, value);
        }

        private int sell;
        public int Sell {
            get => sell;
            set => SetProperty(ref sell, value);
        }

        private int count = 1;
        public int Count {
            get => count;
            set => SetProperty(ref count, value < 1 ? 1 : value);
        }
    }
}
