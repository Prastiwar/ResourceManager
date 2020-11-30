using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Models
{
    public class TraderNpcJobModel : NpcJobModel
    {
        private IList<TradeItemModel> items = new ObservableCollection<TradeItemModel>();
        public IList<TradeItemModel> Items {
            get => items;
            set => SetProperty(ref items, value ?? new ObservableCollection<TradeItemModel>());
        }
    }
}
