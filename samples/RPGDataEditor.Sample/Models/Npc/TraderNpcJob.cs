using System.Collections.Generic;

namespace RPGDataEditor.Sample.Models
{
    public class TraderNpcJob : NpcJob
    {
        public IList<TradeItem> Items { get; set; }
    }
}
