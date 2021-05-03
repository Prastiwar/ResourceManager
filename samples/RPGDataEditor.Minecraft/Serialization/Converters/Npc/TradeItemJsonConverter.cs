using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TradeItemJsonConverter : Core.Serialization.TradeItemJsonConverter
    {
        public override RPGDataEditor.Models.TradeItem ReadJObject(Type objectType, JObject obj)
        {
            RPGDataEditor.Models.TradeItem coreModel = base.ReadJObject(objectType, obj);
            string nbt = obj.GetValue<string>(nameof(TradeItem.Nbt), null);
            TradeItem model = new TradeItem() {
                ItemId = coreModel.ItemId,
                Buy = coreModel.Buy,
                Sell = coreModel.Sell,
                Count = coreModel.Count,
                Nbt = nbt
            };
            return model;
        }

        public override JObject ToJObject(RPGDataEditor.Models.TradeItem value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(TradeItem.Nbt).ToFirstLower(), (value as TradeItem).Nbt);
            return obj;
        }
    }
}
