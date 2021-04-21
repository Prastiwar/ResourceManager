using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class TradeItemModelJsonConverter : ExtendableJsonConverter<TradeItemModel>
    {
        public override TradeItemModel ReadJObject(Type objectType, JObject obj)
        {
            object item = obj.GetValue<object>(nameof(TradeItemModel.ItemId), null);
            int buy = obj.GetValue<int>(nameof(TradeItemModel.Buy), 0);
            int sell = obj.GetValue<int>(nameof(TradeItemModel.Sell), 0);
            int count = obj.GetValue(nameof(TradeItemModel.Count), 1);
            TradeItemModel model = new TradeItemModel() {
                ItemId = item,
                Buy = buy,
                Sell = sell,
                Count = count
            };
            return model;
        }

        public override JObject ToJObject(TradeItemModel value, JsonSerializer serializer) => new JObject() {
                { nameof(TradeItemModel.ItemId).ToFirstLower(), JToken.FromObject(value.ItemId) },
                { nameof(TradeItemModel.Buy).ToFirstLower(), value.Buy },
                { nameof(TradeItemModel.Sell).ToFirstLower(), value.Sell },
                { nameof(TradeItemModel.Count).ToFirstLower(), value.Count }
            };
    }
}
