using System.Collections.Generic;

namespace RpgDataEditor.Models
{
    public class TraderNpcJob : NpcJob
    {
        public IList<TradeItem> Items { get; set; }
    }
}
