using RPGDataEditor.Models;
using System.Collections.Generic;

namespace RPGDataEditor.Minecraft.Models
{
    public class TalkData : RPGDataEditor.Models.TalkData
    {
        public IList<TalkLine> InteractLines { get; set; }

        public IList<TalkLine> HurtLines { get; set; }

        public IList<TalkLine> DeathLines { get; set; }
    }
}
