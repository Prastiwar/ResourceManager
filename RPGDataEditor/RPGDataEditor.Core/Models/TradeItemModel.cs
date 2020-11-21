﻿namespace RPGDataEditor.Core.Models
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

        private int count;
        public int Count {
            get => count;
            set => SetProperty(ref count, value);
        }

        //private CompoundNBT nbt;
        //public CompoundNBT Nbt {
        //    get => nbt;
        //    set => SetProperty(ref nbt, value);
        //}
    }
}
