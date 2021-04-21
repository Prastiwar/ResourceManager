using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class TraderNpcJobModel : NpcJobModel
    {
        public IList<TradeItemModel> Items { get; private set; }
    }
}
