using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Sample.Models;
using System;

namespace RPGDataEditor.Sample.Serialization
{
    public class TradeItemJsonConverter : ExtendableJsonConverter<TradeItem>
    {
        public override TradeItem ReadJObject(Type objectType, JObject obj)
        {
            object item = obj.GetValue<object>(nameof(TradeItem.ItemId), null);
            int buy = obj.GetValue<int>(nameof(TradeItem.Buy), 0);
            int sell = obj.GetValue<int>(nameof(TradeItem.Sell), 0);
            int count = obj.GetValue(nameof(TradeItem.Count), 1);
            TradeItem model = new TradeItem() {
                ItemId = item,
                Buy = buy,
                Sell = sell,
                Count = count
            };
            return model;
        }

        public override JObject ToJObject(TradeItem value, JsonSerializer serializer) => new JObject() {
                { nameof(TradeItem.ItemId).ToFirstLower(), JToken.FromObject(value.ItemId) },
                { nameof(TradeItem.Buy).ToFirstLower(), value.Buy },
                { nameof(TradeItem.Sell).ToFirstLower(), value.Sell },
                { nameof(TradeItem.Count).ToFirstLower(), value.Count }
            };
    }
}
