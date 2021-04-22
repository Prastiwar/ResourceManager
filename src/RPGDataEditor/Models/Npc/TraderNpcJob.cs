using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class TraderNpcJob : NpcJob
    {
        public IList<TradeItem> Items { get; private set; }
    }
}
