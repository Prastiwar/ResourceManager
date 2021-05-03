using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class TradeItem : RPGDataEditor.Models.TradeItem
    {
        private string nbt;
        public string Nbt {
            get => nbt;
            set => SetProperty(ref nbt, value);
        }
    }
}
