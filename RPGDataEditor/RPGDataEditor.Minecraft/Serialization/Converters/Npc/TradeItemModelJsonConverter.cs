using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TradeItemModelJsonConverter : Core.Serialization.TradeItemModelJsonConverter
    {
        public override Core.Models.TradeItemModel ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.TradeItemModel coreModel = base.ReadJObject(objectType, obj);
            string nbt = obj.GetValue<string>(nameof(TradeItemModel.Nbt), null);
            TradeItemModel model = new TradeItemModel() {
                Item = coreModel.Item,
                Buy = coreModel.Buy,
                Sell = coreModel.Sell,
                Count = coreModel.Count,
                Nbt = nbt
            };
            return model;
        }

        public override JObject ToJObject(Core.Models.TradeItemModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(TradeItemModel.Nbt).ToFirstLower(), value.Nbt);
            return obj;
        }
    }
}
