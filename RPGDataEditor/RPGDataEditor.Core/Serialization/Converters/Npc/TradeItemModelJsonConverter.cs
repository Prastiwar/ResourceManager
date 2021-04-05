using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class TradeItemModelJsonConverter : ExtendableJsonConverter<TradeItemModel>
    {
        public override TradeItemModel ReadJObject(Type objectType, JObject obj)
        {
            string item = obj.GetValue<string>(nameof(TradeItemModel.Item), null);
            int buy = obj.GetValue<int>(nameof(TradeItemModel.Buy), 0);
            int sell = obj.GetValue<int>(nameof(TradeItemModel.Sell), 0);
            int count = obj.GetValue(nameof(TradeItemModel.Count), 1);
            TradeItemModel model = new TradeItemModel() {
                Item = item,
                Buy = buy,
                Sell = sell,
                Count = count
            };
            return model;
        }

        public override JObject ToJObject(TradeItemModel value, JsonSerializer serializer) => new JObject() {
                { nameof(TradeItemModel.Item).ToFirstLower(), value.Item },
                { nameof(TradeItemModel.Buy).ToFirstLower(), value.Buy },
                { nameof(TradeItemModel.Sell).ToFirstLower(), value.Sell },
                { nameof(TradeItemModel.Count).ToFirstLower(), value.Count }
            };
    }
}
